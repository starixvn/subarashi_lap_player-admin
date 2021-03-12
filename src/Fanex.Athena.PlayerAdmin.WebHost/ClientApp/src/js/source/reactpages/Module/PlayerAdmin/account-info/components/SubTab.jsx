import React from 'react';
import PropTypes from 'prop-types';

const SubTab = (props) => {
    const {dataList, selectedTab, onChangeTab} = props;

    return (
        <div className="header-tab">
            <div className="content-tab">
                <ul className="tabs">
                    {
                        dataList.map((item) => {
                            return (

                                <li className="tab" key={item.name}>
                                    <a className={item.name === selectedTab ? 'link active' : 'link'}
                                        onClick={() => onChangeTab(item.name)}>
                                        {item.text}
                                    </a>
                                </li>
                            )
                        })
                    }
                </ul>
            </div>
        </div>
    )
}

SubTab.propTypes = {
    dataList: PropTypes.arrayOf(PropTypes.shape({
        name: PropTypes.string,
        text: PropTypes.string
    })).isRequired,
    selectedTab: PropTypes.string.isRequired,
    onChangeTab: PropTypes.func.isRequired
};
SubTab.defaultProps = {};

export default SubTab;