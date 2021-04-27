import React, {useState} from 'react';
import { DropdownButton, DropdownItem } from '@jsroot/components/dropdown';

const BatchUpdateButton = (props) => {
    const [isOpen, setIsOpen] = useState(false);
    const handleToggleDropdown = () => {
        setIsOpen(!isOpen);
    }

    const handleClose = () => {
        setIsOpen(false);
    }

    return (
        <DropdownButton isOpen={isOpen}
            onToggle={handleToggleDropdown}
            buttonLabel={'Batch Update'}
            onClose={handleClose}>
            <DropdownItem>Item1</DropdownItem>
            <DropdownItem>Item2</DropdownItem>
        </DropdownButton>
    )
}

export default BatchUpdateButton;