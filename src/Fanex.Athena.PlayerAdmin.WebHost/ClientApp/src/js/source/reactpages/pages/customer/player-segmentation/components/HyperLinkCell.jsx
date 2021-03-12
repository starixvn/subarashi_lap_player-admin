import React from 'react';
import { Cell } from '@9softstudio/react-grid';

const HyperLinkCell = props => {
    const { name, onClick } = props; 
    return (<Cell className="left-align">
        <div className="username-cell">
            <button className="btn-default btn-link username-link"
                onClick={onClick}>
                {name}
            </button>
        </div>
    </Cell>
    )
}

export default React.memo(HyperLinkCell); 