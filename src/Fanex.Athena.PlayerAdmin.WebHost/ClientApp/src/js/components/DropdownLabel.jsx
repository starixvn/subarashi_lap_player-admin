import React from 'react';
import PropTypes from 'prop-types';
import classNames from 'classnames';

const DropdownLabel = (props) => {
    const { className, text, iconElement, labelStyle, ...attrs } = props;

    const classes = classNames('dropdown-label-container', className, {
        disabled: props.disabled
    });

    return (
        <div className={classes} {...attrs}>
            <span className="dropdown-label" style={labelStyle}>{text}</span>
            {iconElement}
        </div>
    )
}

const propTypes = {
    children: PropTypes.node,
    disabled: PropTypes.bool,
    onClick: PropTypes.func,
    className: PropTypes.string,
    text: PropTypes.any,
    iconElement: PropTypes.any,
    labelStyle: PropTypes.object
};

const defaultProps = {
    disabled: false,
    iconElement: (<i className="dropdown-label-icon chevron down"></i>)
};

DropdownLabel.propTypes = propTypes;
DropdownLabel.defaultProps = defaultProps;

export default React.memo(DropdownLabel);;