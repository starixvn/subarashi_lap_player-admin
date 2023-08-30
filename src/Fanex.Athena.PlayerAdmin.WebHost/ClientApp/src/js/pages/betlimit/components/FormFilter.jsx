import React, { useState } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../actions/index.js';
import { bindActionCreators } from 'redux';
import LangHelper from '@jsroot/lib/langHelper';
import AppData from '@jsroot/lib/AppData';
import Select from 'react-select';

const FormFilter = (props) => {
    const [productId, setProductId] = useState(props.requestParam.ProductId);
    const productList = AppData.productList;

    const onProductChange = (item) => {
        setProductId(item.value);

        if (item.value !== props.requestParam.ProductId) {
            const param = Object.assign({}, props.requestParam, { ProductId: item.value });

            props.setRequestParam(param);
            props.setProductLabel(item.label);
        }
    }

    return (
        <div className='page-filter'>
            <div className="row">
                <div className="col-12 col-md-4 col-sm-12 col-lg-3 col-xl-2">
                    <div className="form-group">
                        <label className="label-text">{LangHelper.getSingleResource('Product')}</label>
                        <Select instanceId="ddlProduct"
                            value={productId}
                            options={productList}
                            onChange={onProductChange} />
                    </div>
                </div>
            </div>
        </div>
    );
}

function mapDispatchToProps(dispatch) {
    const { setRequestParam, setProductLabel } = ActionCreators;
    return bindActionCreators({ setRequestParam, setProductLabel }, dispatch);
}
function mapStateToProps(state) {
    const { requestParam } = state;
    return Object.assign({}, { requestParam });
}

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(FormFilter));