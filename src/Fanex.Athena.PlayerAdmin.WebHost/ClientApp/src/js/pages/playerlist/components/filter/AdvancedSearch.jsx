import React from 'react';
import MultipleSelect from '@9softstudio/react-multiselect';
import PropTypes from 'prop-types';
import Select from 'react-select';
import { baStatusList, specialLevelListWithColor, oddsGroupListWithStyle } from '../../lib/constants';
import LangHelper from '@jsroot/lib/langHelper';
import { getSpecialLevelText, getCustomerClassText } from '../../lib/helpers/utils';

export class AdvancedSearch extends React.Component {
    constructor(props) {
        super(props);

        this.oddsGroupList = Object.keys(oddsGroupListWithStyle).map(item => ({ Id: item, Value: item, Status: true }));
        this.specialLevelList = Object.keys(specialLevelListWithColor).map(item => ({ Id: item, Value: getSpecialLevelText(item), Status: true }));
        this.baStatusList = Object.keys(baStatusList).map(item => ({ value: baStatusList[item], label: LangHelper.getSingleResource(item) }));
        this.customerClassList = props.depositSetting.CustomerClass.map(item => ({ Id: item, Value: getCustomerClassText(item), Status: true }));
        this.commGroupList = props.depositSetting.CommGroups.map(item => ({ Id: item.Id, Value: item.Name, Status: true }));
    }

    _createSelectData(originalList, selectedItemString) {
        const selectedItemList = selectedItemString.split(',').filter(x => x !== '');

        return originalList.map(item => {
            const { Id, Value } = item;

            return { Id, Value, Status: selectedItemList.indexOf(Id) !== -1 };
        });
    }

    handleChange = (name, value) => {
        this.props.onChange(name, value);
    };

    handleChangeSingleDropdown = (name, item) => {
        this.props.onChange(name, item.value);
    };

    render() {
        const { currencyList, param, isAllowedBA, isSubsidiary } = this.props;
        const { currencyIdList, oddsGroup, specialLevel, baFilter } = param;
        const convertedCurrencyList = this._createSelectData(currencyList, currencyIdList);
        const oddsGroupList = this._createSelectData(this.oddsGroupList, oddsGroup);
        const specialLevelList = this._createSelectData(this.specialLevelList, specialLevel);
        const selectedBaStatus = isAllowedBA ? this.baStatusList.find(x => x.value === baFilter) : null;

        const selectedCustomerClass = param.changeOnCustomerClass ? param.customerClass : this.props.depositSetting.CustomerClass.join(",");
        const ddlCustomerClass = this._createSelectData(this.customerClassList, selectedCustomerClass);

        const selectedCommGroups = param.changeOnCommGroups ? param.commGroups : this.props.depositSetting.CommGroups.map(c => c.Id).join(",");
        const ddlCommGroups = this._createSelectData(this.commGroupList, selectedCommGroups);

        return (
            <>
                <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-1">
                    <div className="form-group">
                        <label className="label-text">{LangHelper.getSingleResource("Currency")}</label>
                        <MultipleSelect id="currency"
                            dataSource={convertedCurrencyList}
                            keyField={"Id"}
                            valueField={"Value"}
                            statusField={"Status"}
                            onChange={(currentItem, selectedItemsKey) => this.handleChange('currencyIdList', selectedItemsKey)} />
                    </div>
                </div>

                <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2">
                    <div className="form-group">
                        <label className="label-text">{LangHelper.getSingleResource("OddsGroup")}</label>
                        <MultipleSelect id="oddsGroup"
                            dataSource={oddsGroupList}
                            keyField={"Id"}
                            valueField={"Value"}
                            statusField={"Status"}
                            onChange={(currentItem, selectedItemsKey) => this.handleChange('oddsGroup', selectedItemsKey)} />
                    </div>
                </div>

                <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-1">
                    <div className="form-group">
                        <label className="label-text">{LangHelper.getSingleResource("SpecialLevel")}</label>
                        <MultipleSelect id="specialLevel"
                            dataSource={specialLevelList}
                            keyField={"Id"}
                            valueField={"Value"}
                            statusField={"Status"}
                            onChange={(currentItem, selectedItemsKey) => this.handleChange('specialLevel', selectedItemsKey)} />
                    </div>
                </div>

                {
                    selectedBaStatus !== null &&
                    (
                        <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-1">
                            <div className="form-group">
                                <label className="label-text">{LangHelper.getSingleResource("BA")}</label>
                                <Select instanceId='ddlBaStatus'
                                    name='form-field-name'
                                    value={selectedBaStatus}
                                    options={this.baStatusList}
                                    onChange={(item) => this.handleChangeSingleDropdown('baFilter', item)} />
                            </div>
                        </div>
                    )
                }

                <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-1">
                    <div className="form-group">
                        <label className="label-text">{LangHelper.getSingleResource("CustomerClass")}</label>
                        <MultipleSelect id="ddl-customer-class"
                            dataSource={ddlCustomerClass}
                            keyField={"Id"}
                            valueField={"Value"}
                            statusField={"Status"}
                            onChange={(currentItem, selectedItemsKey) => this.handleChange('customerClass', selectedItemsKey)} />
                    </div>
                </div>
                {!isSubsidiary && <div className="col-12 col-md-4 col-sm-4 col-lg-3 col-xl-2">
                    <div className="form-group">
                        <label className="label-text">{LangHelper.getSingleResource("GroupCommission")}</label>
                        <MultipleSelect id="ddl-comm-group"
                            dataSource={ddlCommGroups}
                            keyField={"Id"}
                            valueField={"Value"}
                            statusField={"Status"}
                            onChange={(currentItem, selectedItemsKey) => this.handleChange('commGroups', selectedItemsKey)} />
                    </div>
                </div>}
            </>
        );
    }
}

const propTypes = {
    param: PropTypes.shape({
        currencyIdList: PropTypes.string.isRequired,
        oddsGroup: PropTypes.string.isRequired,
        specialLevel: PropTypes.string.isRequired,
        baFilter: PropTypes.number.isRequired,
        customerClass: PropTypes.string.isRequired,
        commGroups: PropTypes.string.isRequired
    }).isRequired,
    onChange: PropTypes.func.isRequired
};

AdvancedSearch.propTypes = propTypes;

export default AdvancedSearch;
