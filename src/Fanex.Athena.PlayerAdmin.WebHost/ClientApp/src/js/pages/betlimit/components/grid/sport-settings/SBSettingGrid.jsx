import React from 'react';
import Utility from '../../../lib/utility';
import LangHelper from '@jsroot/lib/langHelper';
import { BetLimitType, InputClassName } from '../../../lib/constants';
import Site from '@jsroot/lib/site';

const NGSettingGrid = (props) => {
    const { setting, data, onChange } = props;
    const setInputRef = props.setInputRef;
    const actionLink = "UserGuide/Document/MaxPayoutPerMatch";
    const url = Site.resolveClientUrl(actionLink);

    const onInputChange = (type, id, value) => {
        const newData = data.map(e => {
            if (e.Id === id) {
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
                if (e.Id === id) {
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
            ref={(e) => setInputRef(type, value.Id, e)}
            autoComplete='off'
            maxLength='14'
            value={value[type]}
            onChange={(e) => onInputChange(type, value.Id, e.target.value)}
            onBlur={(e) => onInputBlur(type, value.Id, e.target.value)}
            disabled={disabled} />
    }

    const buildBody = (editable, data) => {
        let rows = [];

        for (let i = 0; i < data.length; i++) {
            const item = data[i];
            const disabled = !editable;

            rows.push(
                <tr key={item.Id}>
                    <td>{item.BetSettingType}</td>
                    <td className='pa-betlimit-cell'>
                        {buildCell(BetLimitType.MinBet, item, disabled)}
                        {item.BetValueMin > 0 && <span>{` >= ${Utility.formatValue(item.BetValueMin)}`}</span>}
                    </td>
                    <td className='pa-betlimit-cell'>
                        {buildCell(BetLimitType.MaxBet, item, disabled)}
                        {item.BetValueMax > 0 && <span>{` <= ${Utility.formatValue(item.BetValueMax)}`}</span>}
                    </td>
                    <td className='pa-betlimit-cell'>
                        {buildCell(BetLimitType.MaxPerMatch, item, disabled)}
                        {item.MaxPerMatchMax > 0 && <span>{` <= ${Utility.formatValue(item.MaxPerMatchMax)}`}</span>}
                    </td>
                    <td className='pa-betlimit-cell'>
                        {buildCell(BetLimitType.MaxPayoutPerMatch, item, disabled)}
                        {item.MaxPayoutPerMatchMax > 0 && <span>{` <= ${Utility.formatValue(item.MaxPayoutPerMatchMax)}`}</span>}
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
                        <th>{LangHelper.getSingleResource('Sport')}</th>
                        <th>{LangHelper.getSingleResource('MinBet')}</th>
                        <th>{LangHelper.getSingleResource('MaxBet')}</th>
                        <th>{LangHelper.getSingleResource('MaxPerMatch')}</th>
                        <th>{LangHelper.getSingleResource('MaxPayoutPerMatch')}
                            <a href={url} target='_blank' rel='noreferrer'>&nbsp;&nbsp;<span className="icon-info"></span></a>
                        </th>
                    </tr>
                </thead>
                {buildBody(setting.Editable, data)}
            </table>
        </div>
    );
}

export default React.memo(NGSettingGrid);