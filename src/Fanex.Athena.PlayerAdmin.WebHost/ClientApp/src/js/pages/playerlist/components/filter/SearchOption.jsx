import React, { useState } from 'react';
import { DropdownButton, DropdownItem } from '@jsroot/components/dropdown';
import { searchOptions } from '../../lib/constants';
import LangHelper from '@jsroot/lib/langHelper';

const SearchOption = (props) => {
    const { value, onChange } = props;
    const [isOpen, setIsOpen] = useState(false);
    const handleToggleDropdown = () => {
        setIsOpen(!isOpen);
    }

    const handleClose = () => {
        setIsOpen(false);
    }

    const toggleButton = {
        className: 'toggle-outline toggle-outline-light'
    }

    const selectedValue = value || searchOptions.matchPrecise;

    return (
        <DropdownButton className="search-option" isOpen={isOpen} toggleButton={toggleButton}
            onToggle={handleToggleDropdown}
            buttonLabel={(<i className="material-icons">settings</i>)}
            onClose={handleClose}>
            <DropdownItem tag="div">
                <label className="selection-item">
                    <input type="radio" name="searchOption" value={searchOptions.matchPrecise}
                        checked={selectedValue === searchOptions.matchPrecise} onChange={onChange} />
                    &nbsp;&nbsp;{LangHelper.getSingleResource('MatchPrecise')}
                </label>
            </DropdownItem>
            <DropdownItem tag="div">
                <label className="selection-item">
                    <input type="radio" name="searchOption" value={searchOptions.matchPrefix}
                        checked={selectedValue === searchOptions.matchPrefix} onChange={onChange} />
                    &nbsp;&nbsp;{LangHelper.getSingleResource('MatchPrefix')} "<span className="keyword">test</span>"ABCD
                </label>
            </DropdownItem>
            <DropdownItem tag="div">
                <label className="selection-item">
                    <input type="radio" name="searchOption" value={searchOptions.matchCase}
                        checked={selectedValue === searchOptions.matchCase} onChange={onChange} />
                    &nbsp;&nbsp;{LangHelper.getSingleResource('MatchCase')} AB"<span className="keyword">test</span>"CD
                </label>
            </DropdownItem>
        </DropdownButton>
    )
}

export default SearchOption;
