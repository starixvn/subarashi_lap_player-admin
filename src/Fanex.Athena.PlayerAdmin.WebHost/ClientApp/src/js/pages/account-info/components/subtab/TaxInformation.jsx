import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../../actions/index.js';
import { bindActionCreators } from 'redux';
import ClientResourceManager from '@jsroot/lib/clientResourceManager';

export const TaxInformation = (props) => {
    const { licenseeCustId, getTaxInfo } = props;

    const [data, setData] = useState(null);
    const [isShowPlainText, setIsShowPlainText] = useState(false);

    // Sample tax information data - 10 predefined records
    const sampleTaxData = [
        {
            TaxpayerRegistrationNumber: "123.456.789-01",
            PaymentOfIncomeTax: "5,000",
            IndividualIncomeTax: "3,200",
            WithholdingIncomeTax: "1,800"
        },
        {
            TaxpayerRegistrationNumber: "987.654.321-00",
            PaymentOfIncomeTax: "8,500",
            IndividualIncomeTax: "6,700",
            WithholdingIncomeTax: "1,800"
        },
        {
            TaxpayerRegistrationNumber: "111.222.333-44",
            PaymentOfIncomeTax: "2,200",
            IndividualIncomeTax: "1,500",
            WithholdingIncomeTax: "700"
        },
        {
            TaxpayerRegistrationNumber: "555.666.777-88",
            PaymentOfIncomeTax: "4,750",
            IndividualIncomeTax: "3,950",
            WithholdingIncomeTax: "800"
        },
        {
            TaxpayerRegistrationNumber: "999.888.777-66",
            PaymentOfIncomeTax: "9,100",
            IndividualIncomeTax: "7,600",
            WithholdingIncomeTax: "1,500"
        },
        {
            TaxpayerRegistrationNumber: "444.555.666-22",
            PaymentOfIncomeTax: "1,800",
            IndividualIncomeTax: "1,200",
            WithholdingIncomeTax: "600"
        },
        {
            TaxpayerRegistrationNumber: "222.333.444-55",
            PaymentOfIncomeTax: "6,200",
            IndividualIncomeTax: "5,000",
            WithholdingIncomeTax: "1,200"
        },
        {
            TaxpayerRegistrationNumber: "333.444.555-11",
            PaymentOfIncomeTax: "7,300",
            IndividualIncomeTax: "6,100",
            WithholdingIncomeTax: "1,200"
        },
        {
            TaxpayerRegistrationNumber: "666.777.888-33",
            PaymentOfIncomeTax: "3,600",
            IndividualIncomeTax: "2,900",
            WithholdingIncomeTax: "700"
        },
        {
            TaxpayerRegistrationNumber: "777.888.999-44",
            PaymentOfIncomeTax: "5,900",
            IndividualIncomeTax: "4,800",
            WithholdingIncomeTax: "1,100"
        }
    ];

    useEffect(() => {
        // const param = { licenseeCustId };
        // getTaxInfo(param).then(result => setData(result));

        if (licenseeCustId) {
            const recordIndex = licenseeCustId % sampleTaxData.length;
            const selectedRecord = sampleTaxData[recordIndex];
            setData(selectedRecord);
        }
    }, [licenseeCustId]);

    const displayInfo = (value) => isShowPlainText ? value : (value && '*'.repeat(value.length));
    const {
        TaxpayerRegistrationNumber,
        PaymentOfIncomeTax,
        IndividualIncomeTax,
        WithholdingIncomeTax
    } = data || {};

    return (
        <div className="left-side-content tax-infor-container">
            <table className="table">
                <tbody>
                    <tr>
                        <td className="first-cell">
                            {ClientResourceManager.getText('TaxInformation')}
                            <button className="btn-default btn-link btn-icon"
                                onClick={() => setIsShowPlainText(true)}>
                                <i className="material-icons">remove_red_eye</i>
                            </button>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>{ClientResourceManager.getText('TaxpayerRegistrationNumber')}</td>
                        <td>{displayInfo(TaxpayerRegistrationNumber)}</td>
                    </tr>
                    <tr>
                        <td>{ClientResourceManager.getText('PaymentOfIncomeTax')}</td>
                        <td>{displayInfo(PaymentOfIncomeTax)}</td>
                    </tr>
                    <tr>
                        <td>{ClientResourceManager.getText('IndividualIncomeTax')}</td>
                        <td>{displayInfo(IndividualIncomeTax)}</td>
                    </tr>
                    <tr>
                        <td>{ClientResourceManager.getText('WithholdingIncomeTax')}</td>
                        <td>{displayInfo(WithholdingIncomeTax)}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    )
}

function mapDispatchToProps(dispatch) {
    const { getTaxInfo } = ActionCreators;
    return bindActionCreators({ getTaxInfo }, dispatch);
}

function mapStateToProps(state) {
    const { licenseeCustId } = state;
    return Object.assign({}, { licenseeCustId });
}

export default connect(mapStateToProps, mapDispatchToProps)(TaxInformation);