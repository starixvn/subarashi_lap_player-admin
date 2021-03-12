define(['jquery', 'common/site', 'common/popup', './commgroup', 'lib/jquery.colorpicker'],
    function ($, Site, Popup, CustomerCommGroup) {
        'use strict';

        function initialize(titleResource) {
            _me.products = $("#GroupCommProductList").val().split(",");
            Site.initialize();

            initColorPicker();
            CustomerCommGroup.setPageTitle(Site.getSingleResource(titleResource));
            _me.initEvent();
        }

        function initEvent() {
            $('#GroupName').keyup(function () {
                $('#GroupName').removeClass('invalid valid');
                $('#result-mark').addClass('hide');
                $('#Submit').prop('disabled', false);
            });

            $('#Cancel').click(function () {
                CustomerCommGroup.setPageTitle(Site.getSingleResource('CommissionGroupList'));
                Site.redirect(Site.resolveClientUrl('CustomerComGroup/Index'));
            });

            // Commission dropdownlist change
            _me.products.forEach(function (p, i) {
                $('#' + p + 'Comm1').on('change', function () {
                    commissionOnChange($(this).val(), p + 'Comm');
                }).change();
            });
        }

        function checkAvailableGroup(groupName) {
            if (groupName.length < 1) {
                displayGroupNameError(Site.getSingleResource('GroupNameNotEmptyMsg'));
                return;
            }

            if (groupName.length >= 100) {
                displayGroupNameError(Site.getSingleResource('GroupNameNotExceed100CharactersMsg'));
                return;
            }

            if ($.trim(groupName).toLowerCase() === 'none') {
                displayGroupNameError(Site.getSingleResource('ChooseGroupNameMsg'));
                return;
            }

            if (!groupName.match('^[a-zA-Z0-9 ]+$')) {
                displayGroupNameError(Site.getSingleResource('GroupNameOnlyContainCharactersAndNumberMsg'));
                return;
            }

            validateGroupName($.trim(groupName));
        }

        function validateGroupName(groupName) {
            const url = 'CustomerComGroup/Validate?groupName=' + groupName;

            $.ajax({
                url: Site.resolveClientUrl(url),
                type: 'GET',
                dataType: 'html',
                cache: false,
                beforeSend: function () {
                    Site.blockUI();
                },
                success: function (result) {
                    if (result === 'OK') {
                        $('#GroupName').removeClass('invalid').addClass('valid');
                        $('#result-mark').removeClass('hide');
                        $('#Submit').prop('disabled', false);
                        $('.result-msg').addClass('hide');
                    }
                    else {
                        displayGroupNameError(Site.getSingleResource('GroupNameExistedMsg'));
                    }

                    Site.unBlockUI();
                }
            });
        }

        function displayGroupNameError(errorMsg) {
            $('#GroupName').addClass('invalid').focus();
            $('#result-mark').addClass('hide');
            $('#Submit').prop('disabled', true);

            var resultMsg = $('.result-msg');
            resultMsg.addClass('error').removeClass('hide success');
            resultMsg.html(errorMsg);
        }

        function commissionOnChange(value, idName) {
            const comm2 = '#' + idName + '2', comm3 = '#' + idName + '3';

            if (Number(value) === 15) {
                $(comm2).val(0);
                $(comm2).selectList('update', {
                    isDisabled: true,
                    selectionId: 'select-list-' + idName + '2',
                    labelText: '0'
                });

                $(comm3).val(0);
                $(comm3).selectList('update', {
                    isDisabled: true,
                    selectionId: 'select-list-' + idName + '3',
                    labelText: '0'
                });
            } else {
                $(comm2).selectList('disabled', false);
                $(comm3).selectList('disabled', false);
            }
        }

        function submit(formId) {
            for (var i = 0; i < _me.products.length; i++) {
                var p = _me.products[i];
                initCommParam('#' + p + 'Comm');
            }

            initDefaultParam(formId);
        }

        function initCommParam(id) {
            const comm1 = parseInt($(id + '1').val(), 10);
            const comm2 = parseInt($(id + '2').val(), 10);
            const comm3 = parseInt($(id + '3').val(), 10);

            let comm = parseFloat((comm1 + comm2 / 10 + comm3 / 100) / 100).toFixed(4);
            comm = isNaN(comm) ? 0 : comm;

            $(id).val(comm);
        }

        function initDefaultParam(formId) {
            const isDefault = $('#IsDefaultGroup').is(':checked');
            const defaultGroupId = parseInt($('#DefaultGroupId').val(), 10);
            const currentGroupId = parseFloat($('#GroupCommId').val());
            $('#IsDefaultGroup').val(isDefault);
            $('#IsResetDefault').val(false);
            if (isDefault && defaultGroupId !== currentGroupId && defaultGroupId !== 0) {
                showConfirmAction(Site.getSingleResource('GroupSetAsDefaultMsg'), function () {
                    $('#IsResetDefault').val(true);
                    Site.blockUI();
                    $(formId).submit();
                });
            } else {
                Site.blockUI();
                $(formId).submit();
            }
        }

        function goToCustomerList(groupCommId) {
            CustomerCommGroup.goToCustomerList(groupCommId);
        }

        function showConfirmAction(message, callback) {
            const options = {
                modal: true,
                title: Site.getSingleResource('Confirm'),
                height: 'auto',
                id: 'confirm-action',
                width: 450
            };

            const buttons = [
                {
                    text: Site.getSingleResource('OK'),
                    isClosedOnClick: true,
                    click: callback,
                    classes: 'btn btn-submit'
                }, {
                    text: Site.getSingleResource('Cancel'),
                    isClosedOnClick: true,
                    classes: 'btn btn-cancel'
                }];

            Popup.openPopupWithButton('<div><p>' + message + '</p></div>', options, buttons);
        }

        function initColorPicker() {
            $('#GroupColor').colorPicker({
                transparency: true
            });
        }

        const _me = {
            initialize: initialize,
            initEvent: initEvent,
            submit: submit,
            checkAvailableGroup: checkAvailableGroup,
            goToCustomerList: goToCustomerList
        };

        return _me;
    }
);
