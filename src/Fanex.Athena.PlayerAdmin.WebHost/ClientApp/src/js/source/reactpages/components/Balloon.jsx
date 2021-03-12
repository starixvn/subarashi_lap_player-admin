import React, { PureComponent } from 'react';
import Overlay from './Overlay';
import PropTypes from 'prop-types';

class Balloon extends PureComponent {
    constructor(props) {
        super(props);

        this.box = props.forwardedRef;

        this.state = {
            top: 0,
            left: 0
        }
    }

    static propTypes = {
        isOpen: PropTypes.bool,
        msg: PropTypes.string,
        targetTop: PropTypes.number,
        targetLeft: PropTypes.number,
        width: PropTypes.number
    }

    static defaultProps = {
        isOpen: false,
        msg: '',
        targetTop: 0,
        targetLeft: 0,
        width: 0,
    }

    componentDidUpdate(prevProps) {
        const { targetTop, targetLeft, width } = this.props;

        if (!prevProps.isOpen && this.props.isOpen
            && (prevProps.targetTop !== targetTop || prevProps.targetLeft !== targetLeft || prevProps.msg !== this.props.msg)) {
            this.setState({
                left: targetLeft - width / 2,
                top: targetTop - (this.box.current ? this.box.current.clientHeight : 0) - 20
            });
        }
    }

    render() {
        const { msg, targetTop, targetLeft, width, isOpen } = this.props;
        const { top, left } = this.state;

        return (
            <div className='balloon' style={{ top: top, left: left, display: isOpen ? 'block' : 'none' }} ref={this.box} >
                <div className='balloon-msg' style={{ width: width }}>{msg}</div>
                <div className='balloon-arrow-border' style={{ left: (width - 10) / 2 }}></div>
                <div className='balloon-arrow' style={{ left: (width - 10) / 2 }}></div>
            </div>
        )
    }
}

const balloonWithRef = React.forwardRef((props, ref) => {
    return <Balloon {...props} forwardedRef={ref} />
})

export default balloonWithRef;