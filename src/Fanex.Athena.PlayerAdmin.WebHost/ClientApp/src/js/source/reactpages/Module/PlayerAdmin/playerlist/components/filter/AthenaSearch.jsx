import React from 'react';
import PropTypes from 'prop-types';
import LangHelper from '@jsroot/lib/langHelper';

const AthenaSearch = (props) => {
    const { bankAccountNumber, email, onChange, onKeyPress } = props;

    return (
        <>
            <div className="col-12 col-md-4 col-sm-4 col-lg-3 col-xl-2">
                <div className="form-group">
                    <label className="label-text">{LangHelper.getSingleResource("BankAccountNumber")}</label>
                    <input className="input-text" id="bankAccountNumber" name="bankAccountNumber" type="text" value={bankAccountNumber}
                        autoComplete="off" onChange={onChange} onKeyPress={onKeyPress} 
                        placeholder={LangHelper.getSingleResource('PlaceholderBankAccount')} />
                </div>
            </div>

            <div className="col-12 col-md-4 col-sm-4 col-lg-3 col-xl-2">
                <div className="form-group">
                    <label className="label-text">{LangHelper.getSingleResource("Email")}</label>
                    <input className="input-text" id="email" name="email" type="text" value={email}
                        autoComplete="off" onChange={onChange} onKeyPress={onKeyPress}
                        placeholder={LangHelper.getSingleResource('PlaceholderEmail')} />
                </div>
            </div>
        </>
    )
}

const propTypes = {
    bankAccountNumber: PropTypes.string.isRequired,
    email: PropTypes.string.isRequired,
    onChange: PropTypes.func.isRequired,
    onKeyPress: PropTypes.func.isRequired
};

AthenaSearch.propTypes = propTypes;

export default React.memo(AthenaSearch);