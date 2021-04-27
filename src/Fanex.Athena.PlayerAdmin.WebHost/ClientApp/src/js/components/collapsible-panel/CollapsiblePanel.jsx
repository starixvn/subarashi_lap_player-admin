import React from 'react';
import PropTypes from 'prop-types';

class CollapsiblePanel extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            isCollapsed: false
        };
    };

    static propTypes = {
        stickyFooter: PropTypes.node,
        onCollapse: PropTypes.func,
        onExpand: PropTypes.func
    }

    onClick = () => {
        const newState = !this.state.isCollapsed;

        const { onCollapse, onExpand } = this.props;
        if (newState) {
            onCollapse && onCollapse();
        }

        this.setState({ isCollapsed: newState }, () => {
            window.dispatchEvent(new Event('resize'));
            if (!newState) {
                onExpand && onExpand();
            }
        });
    }

    render() {
        const { stickyFooter, iconContainerStyle, iconToggle } = this.props;
        const { tag, className, style, ...attrs } = iconToggle;
        const IconTag = tag;
        const iconStyle = style || {
            transform: this.state.isCollapsed ? null : 'rotate(180deg)',
            color: '#fff',
            textAlign: 'center',
            background: '#4c6274',
            height: "15px",
            width: "20px",
            float: 'left'
        };

        return (
            <div className={`${this.props.className}`}>
                <div style={{ display: this.state.isCollapsed ? 'none' : 'block' }}>
                    {this.props.children}
                </div>
                <div className='caret-block'>
                    {stickyFooter}
                </div>
                <div className="filter-handle" style={iconContainerStyle} onClick={this.onClick}>
                    <IconTag className={className} style={iconStyle} {...attrs} />
                </div>
            </div>
        )
    }
}

const propTypes = {
    stickyFooter: PropTypes.node,
    onCollapse: PropTypes.func,
    onExpand: PropTypes.func,
    iconToggle: PropTypes.object
}
const defaultProps = {
    iconContainerStyle: {
        height: '15px',
        width: '20px',
        cursor: 'pointer',
        position: 'absolute',
        left: 'calc(50% - 10px)',
        bottom: 0,
        background: '#4c6274',
        textAlign: 'center'
    },
    iconToggle: {
        tag: 'i',
        className: 'icon-arrow',
        style: null
    }
};

CollapsiblePanel.propTypes = propTypes;
CollapsiblePanel.defaultProps = defaultProps;

export default CollapsiblePanel;