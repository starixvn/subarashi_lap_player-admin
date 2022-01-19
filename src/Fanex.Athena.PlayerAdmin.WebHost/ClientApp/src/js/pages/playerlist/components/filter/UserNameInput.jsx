import React, { useState, useRef } from 'react';
import { DropdownMenu, DropdownItem } from '@jsroot/components/dropdown';
import Timer from '@jsroot/components/utils/timer';

export const UserNameInput = React.forwardRef((props, userNameRef) => {
    const { onSelectItem, minChar, getSuggestionList, searchOption, onChange, ...inputAttributes } = props;

    const [isOpenSuggestion, setIsOpenSuggestion] = useState(false);
    const [suggestionList, setSuggestionList] = useState([]);
    const [dropdownStyle, setDropdownStyle] = useState(null);

    const cachedDataRef = useRef({});

    const handleChange = () => {
        const element = userNameRef.current;
        if (!element) {
            return;
        }

        const rect = element.getBoundingClientRect();
        const style = {
            left: rect.left,
            top: rect.top + rect.height,
            width: rect.width,
            position: 'fixed'
        };

        const handleSuggestionResult = (resultData) => {
            setDropdownStyle(style);
            setSuggestionList(resultData);
            const hasData = resultData && resultData.length;
            setIsOpenSuggestion(!!hasData);
        }

        const { value } = element;

        if (getSuggestionList && value.length >= minChar) {
            const cachedData = cachedDataRef.current;
            const cacheKey = value + '_' + searchOption;

            if (cachedData[cacheKey]) {
                const result = cachedData[cacheKey];
                handleSuggestionResult(result);
            }
            else {
                const param = { keyword: value, searchType: searchOption };
                getSuggestionList(param).then(response => {
                    const result = response.data || [];
                    handleSuggestionResult(result);

                    cachedData[cacheKey] = result;
                })
            }
        }
        else {
            setIsOpenSuggestion(false);
            setSuggestionList([]);
        }

        onChange && onChange(value);
    }

    const debounceChange = Timer.debounce(handleChange, 300);

    const handleSelectSuggestionItem = (value) => {
        onSelectItem && onSelectItem(value);
        handleCloseDropdown();
    }

    const handleCloseDropdown = () => {
        setIsOpenSuggestion(false);
    }

    return (
        <div className="pa-username">
            <input className="input-text" id="userName" name="userName" type="text" ref={userNameRef}
                autoComplete="off" onChange={debounceChange} {...inputAttributes} />
            <div className="dropdown">
                <DropdownMenu isOpen={isOpenSuggestion} style={dropdownStyle}>
                    {
                        suggestionList.map(item => {
                            return <DropdownItem key={item} onClick={() => handleSelectSuggestionItem(item)}>{item}</DropdownItem>
                        })
                    }
                </DropdownMenu>
            </div>
        </div>
    )
});

export default UserNameInput;