import React from 'react';
import Utility from '../../../lib/utility';
import { BetLimitType, InputClassName } from '../../../lib/constants';

const I4DSettingGrid = (props) => {
    const { setting, data, onChange, getHeaderName } = props;
    const setInputRef = props.setInputRef;

    const onInputChange = (type, id, value) => {
        const newData = data.map(e => {
            if (e.BetType === id) {
                const newItem = { ...e };
                newItem[type] = value;

                return newItem;
            }

            return e;
        });

        onChange(newData);
    }

    const onInputBlur = (type, id, value) => {
        const newValue = Utility.formatValue(Utility.getValueAsNumber(value));

        if (newValue !== value) {
            const newData = data.map(e => {
                if (e.BetType === id) {
                    const newItem = { ...e };
                    newItem[type] = newValue;

                    return newItem;
                }

                return e;
            });

            onChange(newData);
        }
    }

    const buildCell = (type, value, disabled) => {
        const isChanged = value[type] !== value['Original' + type];

        return <input className={isChanged ? InputClassName.OnChangedClassName : InputClassName.ClassName}
            type='text'
            ref={(e) => setInputRef(type, value.BetType, e)}
            autoComplete='off'
            maxLength='14'
            value={value[type]}
            onChange={(e) => onInputChange(type, value.BetType, e.target.value)}
            onBlur={(e) => onInputBlur(type, value.BetType, e.target.value)}
            disabled={disabled} />
    }

    const buildBody = (editable, data) => {
        let rows = [];

        for (let i = 0; i < data.length; i++) {
            const item = data[i];
            const disabled = !editable;

            rows.push(
                <tr key={item.BetType}>
                    <td>{item.BetSettingType}</td>
                    <td className='pa-betlimit-cell'>
                        {buildCell(BetLimitType.MinBet, item, disabled)}
                        {item.BetValueMin > 0 && <span>{` >= ${Utility.formatValue(item.BetValueMin)}`}</span>}
                    </td>
                    <td className='pa-betlimit-cell'>
                        {buildCell(BetLimitType.MaxBet, item, disabled)}
                        {item.BetValueMax > 0 && <span>{` <= ${Utility.formatValue(item.BetValueMax)}`}</span>}
                    </td>
                </tr>
            )
        }

        return <tbody>{rows}</tbody>;
    }

    return (
        <div className="pa-betlimit-grid">
            <table className='table'>
                <thead>
                    <tr>
                        <th>{getHeaderName('Sport')}</th>
                        <th>{getHeaderName('MinBet')}</th>
                        <th>{getHeaderName('MaxBet')}</th>
                    </tr>
                </thead>
                {buildBody(setting.Editable, data)}
            </table>
        </div>
    );
}

export default React.memo(I4DSettingGrid);