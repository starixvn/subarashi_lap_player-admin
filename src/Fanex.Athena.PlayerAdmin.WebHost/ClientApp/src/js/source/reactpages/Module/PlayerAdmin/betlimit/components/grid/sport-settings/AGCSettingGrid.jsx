import React from 'react';
import Utility from '../../../lib/utility';
import LangHelper from '@jsroot/lib/langHelper';

const AGCSettingGrid = (props) => {
    const { data, isEnabled, group } = props;

    const buildBody = () => {
        let rows = [];

        for (let i = 0; i < data.length; i++) {
            const game = data[i];
            const gameGroup = game.Groups.find(x => x.GroupType === group);

            rows.push(
                <tr key={game.GameType}>
                    <td>{game.Name}</td>
                    <td className={isEnabled ? '' : 'disabled-text'}>{gameGroup ? Utility.formatValue(gameGroup.Min) : ''}</td>
                    <td className={isEnabled ? '' : 'disabled-text'}>{gameGroup ? Utility.formatValue(gameGroup.Max) : ''}</td>
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
                        <th>{LangHelper.getSingleResource('GameType')}</th>
                        <th>{LangHelper.getSingleResource('MinBet')}</th>
                        <th>{LangHelper.getSingleResource('MaxBet')}</th>
                    </tr>
                </thead>
                {buildBody()}
            </table>
        </div>
    );
}

export default React.memo(AGCSettingGrid);