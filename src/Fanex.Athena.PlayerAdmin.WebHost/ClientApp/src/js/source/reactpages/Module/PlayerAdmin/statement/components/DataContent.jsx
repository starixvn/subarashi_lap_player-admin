import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import LangHelper from '@jsroot/lib/langHelper';
import Service from '../lib/service';
import NoDataTitle from '@jsroot/components/NoDataTitle';
import Timer from '@jsroot/components/utils/timer';

const MinimumContentHeight = 200;

export class DataContent extends PureComponent {
    constructor(props) {
        super(props);

        this.contentRef = React.createRef();

        this.state = {
            src: '',
            height: 0
        }

        this.resize = Timer.debounce(this._handleResize, 15).bind(this);
    }

    componentDidUpdate(prevProps, prevState) {
        if (prevProps.requestParam !== this.props.requestParam) {
            this._getUrl(this.props.requestParam);
        }
    }

    componentDidMount() {
        this._getUrl(this.props.requestParam);
        this._handleResize();
        window.addEventListener('resize', this.resize);
    }

    componentWillUnmount() {
        window.removeEventListener('resize', this.resize);
    }

    _handleResize(event) {
        event && event.preventDefault();

        if (this.contentRef.current) {
            const height = window.innerHeight - this.contentRef.current.getBoundingClientRect().top - 20;

            this.setState({ height: height > MinimumContentHeight ? height : MinimumContentHeight });
        }
    }

    _updateSrc(src) {
        this.setState({ src: src });
    }

    _getUrl(param) {
        Service.getData(param, this._updateSrc.bind(this));
    }

    render() {
        const { src, height } = this.state;

        return (
            <div>
                <div style={{ height: height }} ref={this.contentRef}>
                {
                    src
                    ? <iframe frameBorder="0" src={src} width="100%" height="100%" />
                    : <NoDataTitle />
                }
                </div>
            </div>
        );      
    }
}

function mapStateToProps(state) {
    const { requestParam } = state;
    return Object.assign({}, { requestParam });
};

export default connect(mapStateToProps)(DataContent);