import React from 'react';
import { Row, Cell } from '@9softstudio/react-grid';
import SelectionCell from './SelectionCell';
import VipSwitcherCell from '../../../../components/VipSwitcherCell';

export const PlayerRow = (props) => {
    const { subsidiary, externalId, systemId, isBA, index, custId, showSubsidiary, customerClass } = props;
    const canEditVip = !(isBA);

    return (<Row key={`player_row_${custId}`}>
        {canEditVip ? <SelectionCell rowIndex={index} /> : <Cell />}
        {showSubsidiary && <Cell className="left-align">{subsidiary}</Cell>}
        <Cell className="left-align">{externalId}</Cell>
        <Cell className="left-align">{systemId}</Cell>
        <Cell className="center-align">{customerClass}</Cell>
        <VipSwitcherCell index={index} />
    </Row>)
}

export default React.memo(PlayerRow);