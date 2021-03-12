import React from 'react';
import Popup from '../popup';
import PropTypes from 'prop-types';
import {
    ICON,
    errorIconMarkup,
    warningIconMarkup,
    successIconMarkup,
} from './icon';

const PREDEFINED_ICONS = ["error", "warning", "success", "info"];

const ICON_CONTENTS = {
    error: errorIconMarkup(),
    warning: warningIconMarkup(),
    success: successIconMarkup(),
}

export class SweetAlert extends React.Component {
    constructor(props) {
        super(props);
    }

    _buildButtonGroup(buttons) {
        return (
            <div className="al-button-container">
                {buttons.map((btnItem, index) => {
                    const { text, ...attrs } = btnItem;
                    return (<button key={index} type="button" {...attrs}>{text}</button>)
                })}
            </div>
        )
    }

    _buildIcon() {
        const { icon } = this.props;
        const className = `${ICON} ${ICON}--${icon}`;

        if (PREDEFINED_ICONS.includes(icon)) {
            return (
                <div className={className} dangerouslySetInnerHTML={{ __html: ICON_CONTENTS[icon] }}></div>
            )
        }

        return null;
    }

    render() {
        const { buttons, children, ...props } = this.props;

        return (
            <div className="sweet-alert">
                <Popup {...props}>
                    {this._buildIcon()}
                    {children}
                    {this._buildButtonGroup(buttons)}
                </Popup>
            </div>
        )
    }
}

SweetAlert.propTypes = {
    width: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
    height: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
    title: PropTypes.string,
    isShow: PropTypes.bool,
    onClose: PropTypes.func,
    isHtml: PropTypes.bool,
    buttons: PropTypes.arrayOf(PropTypes.object)
}

SweetAlert.defaultProps = {
    width: 400,
    height: 200,
    isShow: false,
    isHtml: false,
    title: '',
    buttons: []
}

export default SweetAlert;