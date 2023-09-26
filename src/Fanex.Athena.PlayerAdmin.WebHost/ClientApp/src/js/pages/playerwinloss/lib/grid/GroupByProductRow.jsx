import React, { useEffect, useState } from 'react';
import { Row, Cell } from '@9softstudio/react-grid';
import TreeViewGridRowNode from './TreeViewGridRowNode';

export const GroupByProductRow = (props) => {
    const [isBreakdown, setIsBreakDown] = useState(false);

    useEffect(() => {
    }, []);

    const { data, option } = props;
    if (!data) {
        return null;
    }

    const _onclick = () => {
        setIsBreakDown(!isBreakdown);
    }
    const arrowClass = isBreakdown ? 'down-arrow' : 'right-arrow';
    const text = data.item;
    const datalist = data.children;
    return (
        <>
            <Row className="group-product-row" key={`row-${text}`}>
                <Cell colSpan={11}>
                    <span className={`arrow-icon ${arrowClass}`} onClick={_onclick}></span>
                    <span className='group-text'>{text}</span>  
                </Cell>
            </Row>
            {isBreakdown && datalist && datalist.length > 0 && datalist.map(item => (
                <TreeViewGridRowNode 
                    key={`row-groupby-${item.item.rowIndex}-${item.item.groupBy}`} 
                    data={item} 
                    option={option}/>
                ))}
        </>
    );
}

export default GroupByProductRow;

