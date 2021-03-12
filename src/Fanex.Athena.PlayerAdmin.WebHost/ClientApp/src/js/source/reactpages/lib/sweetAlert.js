import React from 'react';
import LangHelper from './langHelper';

export const predefinedIcons = {
    error: 'error',
    warning: 'warning',
    success: 'success'
}

export const defaultSubmitButton = {
    text: LangHelper.getSingleResource('Submit'),
    className: 'btn btn-apply'
}

export const defaultCancelButton = {
    text: LangHelper.getSingleResource('Cancel'),
    className: 'btn'
}

export const defaultOkButton = {
    text: LangHelper.getSingleResource('OK'),
    className: 'btn btn-apply'
}

export const generateDefaultConfirmOption = (content, onClose, submitProps, cancelProps, icon = predefinedIcons.warning, customOption = {}) => {
    return {
        buttons: [
            { ...defaultSubmitButton, ...submitProps },
            { ...defaultCancelButton, ...cancelProps }
        ],
        icon,
        isShow: true,
        children: (<p className="center-align">{content}</p>),
        onClose,
        ...customOption
    }
}

export const generateDefaultAlertOption = (content, onClose, onClick, icon = predefinedIcons.success) => {
    return {
        buttons: [
            {
                text: LangHelper.getSingleResource('OK'),
                className: 'btn btn-apply',
                onClick: onClick || onClose
            }
        ],
        icon,
        isShow: true,
        children: (<p className="center-align">{content}</p>),
        onClose
    }
}