import React from 'react';
import MultipleSelect from '@legion-lib/react-multiselect';
import PropTypes from 'prop-types';
import { specialLevelListWithColor, responsiveFilterColumn } from '../../lib/constants';
import LangHelper from '@jsroot/lib/langHelper';
import { getSpecialLevelText } from '../../lib/helpers/utils';

export class AdvancedSearch extends React.Component {
    constructor(props) {
        super(props);
        this.specialLevelList = Object.keys(specialLevelListWithColor).map(item => ({ Id: item, Value: getSpecialLevelText(item), Status: true }));
        this.riskLevelList = props.depositSetting.RiskLevels.map(item => ({ Id: item.Id, Value: item.Name, Status: true, valueDecoration: `risk-level ${item.Key}` }));
        this.commGroupList = props.depositSetting.CommGroups.map(item => ({ Id: item.Id, Value: item.Name, Status: true }));
        this.oddsGroupList = props.depositSetting.OddsGroups.map(item => ({ Id: item, Value: item, Status: true }));
    }

    _createSelectData(originalList, selectedItemString) {
        const selectedItemList = selectedItemString.split(',').filter(x => x !== '');
        return originalList.map(item => {
            const { Id, Value } = item;
            return { Id, Value, Status: selectedItemList.indexOf(Id) !== -1 };
        });
    }

    _createRiskLevelSelectData(originalList, selectedItemString) {
        const selectedItemList = selectedItemString.split(',').filter(x => x !== '');
        return originalList.map(item => {
            const { Id, Value, valueDecoration } = item;
            return { Id, Value, valueDecoration, Status: selectedItemList.indexOf(Id) !== -1 };
        });
    }

    handleChange = (name, value) => {
        this.props.onChange(name, value);
    };

    handleCustomerClassChange = value => {
        this.props.onChange("riskLevels", value);
        var customerClassSelected = this.props.depositSetting.RiskLevels.filter(r => value.includes(r.Id)).map(r => r.CustomerClassList.join(',')).join(',');
        this.props.onChange("customerClass", customerClassSelected);
    };

    handleChangeSingleDropdown = (name, item) => {
        this.props.onChange(name, item.value);
    };

    render() {
        const { currencyList, param, isSubsidiary, type } = this.props;
        const { currencyIdList, oddsGroup, specialLevel, riskLevels, commGroups } = param;

        const convertedCurrencyList = this._createSelectData(currencyList, currencyIdList);
        const oddsGroupList = this._createSelectData(this.oddsGroupList, oddsGroup);
        const specialLevelList = this._createSelectData(this.specialLevelList, specialLevel);
        const riskLevelList = this._createRiskLevelSelectData(this.riskLevelList, riskLevels);
        const commGroupList = this._createSelectData(this.commGroupList, commGroups);

        return (
            <>
                <div className={responsiveFilterColumn["currency"][type]}>
                    <div className="form-group">
                        <label className="label-text">{LangHelper.getSingleResource("Currency")}</label>
                        <MultipleSelect id="currency"
                            language={LangHelper.currentLanguage}
                            dataSource={convertedCurrencyList}
                            keyField={"Id"}
                            valueField={"Value"}
                            statusField={"Status"}
                            onChange={(currentItem, selectedItemsKey) => this.handleChange('currencyIdList', selectedItemsKey)} />
                    </div>
                </div>

                <div className={responsiveFilterColumn["oddsGroup"][type]}>
                    <div className="form-group">
                        <label className="label-text">{LangHelper.getSingleResource("OddsGroup")}</label>
                        <MultipleSelect id="oddsGroup"
                            language={LangHelper.currentLanguage}
                            dataSource={oddsGroupList}
                            keyField={"Id"}
                            valueField={"Value"}
                            statusField={"Status"}
                            onChange={(currentItem, selectedItemsKey) => this.handleChange('oddsGroup', selectedItemsKey)} />
                    </div>
                </div>

                <div className="col-12 col-md-4 col-sm-6 col-lg-3 col-xl-2">
                    <div className="form-group">
                        <label className="label-text">{LangHelper.getSingleResource("SpecialLevel")}</label>
                        <MultipleSelect id="specialLevel"
                            language={LangHelper.currentLanguage}
                            dataSource={specialLevelList}
                            keyField={"Id"}
                            valueField={"Value"}
                            statusField={"Status"}
                            onChange={(currentItem, selectedItemsKey) => this.handleChange('specialLevel', selectedItemsKey)} />
                    </div>
                </div>

                <div className={responsiveFilterColumn["riskLevel"][type]}>
                    <div className="form-group">
                        <label className="label-text">{LangHelper.getSingleResource("RiskLevel")}</label>
                        <MultipleSelect id="ddl-risk-level"
                            language={LangHelper.currentLanguage}
                            dataSource={riskLevelList}
                            keyField={"Id"}
                            valueField={"Value"}
                            statusField={"Status"}
                            onChange={(currentItem, selectedItemsKey) => this.handleCustomerClassChange(selectedItemsKey)} />
                    </div>
                </div>

                {!isSubsidiary && <div className={responsiveFilterColumn["groupCommission"][type]}>
                    <div className="form-group">
                        <label className="label-text">{LangHelper.getSingleResource("CommissionGroup")}</label>
                        <MultipleSelect id="ddl-comm-group"
                            language={LangHelper.currentLanguage}
                            dataSource={commGroupList}
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
        customerClass: PropTypes.string.isRequired,
        commGroups: PropTypes.string.isRequired
    }).isRequired,
    onChange: PropTypes.func.isRequired
};

AdvancedSearch.propTypes = propTypes;

export default AdvancedSearch;
