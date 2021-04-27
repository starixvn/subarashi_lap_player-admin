import React from 'react';
import ReactDOM from 'react-dom';
import Popup from './popup/Popup';
import PropTypes from 'prop-types';

export default class UserGuide extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            isShow: false
        }
    };

    static propTypes = {
        title: PropTypes.string,
        className: PropTypes.string,
        contentTitle: PropTypes.string,
        isHtml: PropTypes.bool
    }

    static defaultProps = {
        className: 'icon-book right',
        isHtml: true
    }

    onClickHandler = () => {
        this.setState({
            isShow: true
        });

        this.props.onClick && this.props.onClick();
    }

    onCloseHandler = () => {
        this.setState({
            isShow: false
        });
    }

    render() {
        const { className, title, content, contentTitle, isHtml } = this.props;
        const isShow = this.state.isShow && content && (React.isValidElement(content) || content.length > 0) ? true : false;
        return (<div>
            <i className={className} onClick={this.onClickHandler} title={title}></i>
            <Popup width={640} height={480} isShow={isShow} title={contentTitle} onClose={this.onCloseHandler} isHtml={isHtml}>
                {content}
            </Popup>
        </div>);
    }
};