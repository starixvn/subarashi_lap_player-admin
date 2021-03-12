import React from 'react';
import { Cell } from '@9softstudio/react-grid';
import CheckBox from '@jsroot/components/CheckBox';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../../actions';

const SelectionCell = props => {
    const { rowIndex, playerList, setPlayerList } = props;
    const dataItem = playerList[rowIndex];
   return  (<Cell className="center-align">
            {!dataItem.isMarkVoid && <CheckBox id={`selectPlayer_${dataItem.custId}`}
                checked={dataItem.isSelected === true}
                onChange={(checked) => {
                    const newDataItem = {
                        ...dataItem,
                        ...{ isSelected: checked }
                    }
                    const newPlayerList = [...playerList];
                    newPlayerList[rowIndex] = newDataItem;
                    setPlayerList([...newPlayerList])
                }} />}</Cell>);
}

function mapDispatchToProps(dispatch) {
    const { setPlayerList } = ActionCreators;
    return bindActionCreators({ setPlayerList }, dispatch);
}

function mapStateToProps(state) {
    const { playerList } = state;
    return Object.assign({}, { playerList });
};

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(SelectionCell));