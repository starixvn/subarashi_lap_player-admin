import React from 'react';
import LangHelper from '@jsroot/lib/langHelper';

const Note = () => {
    return (
        <div className="row">
            <div className="col-1" style={{ flex: "0 0 4%" }}>
                {LangHelper.getSingleResource('Note')}:
                    </div>
            <div className="col-9">
                <p>* Support searching void tickets only</p>
                <p>* Input username to return results faster</p>
                <p>* Support data up to 1 months</p>
            </div>
        </div>
    )
}

export default Note;
