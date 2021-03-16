import React, { useReducer, createContext } from "react";

export const SETFILES = "SETFILES";
export const REMOVEFILES = "REMOVEFILES";
export const EDITFILE = "EDITFILE";
export const DELETEFILE = "DELETEFILE";
export const SORTFILE = "SORTFILE";

export const FileContext = createContext();

const fileReducer = (state, action) => {
  switch (action.type) {
    case SETFILES:
      return {
        ...state,
        files: action.files,
      };
    case REMOVEFILES:
      return {
        ...state,
        files: action.files,
      };
    case EDITFILE:
      const index = state.files.findIndex((file) => file.id === action.fid);
      const newArr = [...state.files];
      newArr[index].fileName = action.name;
      return {
        ...state,
        files: newArr,
      };
    case DELETEFILE:
      const filteredFiles = state.files.filter(
        (fold) => fold.id !== action.fid
      );
      return {
        ...state,
        files: filteredFiles,
      };
    case SORTFILE:
      const fileArr = state.files.slice();
      fileArr.sort(function (a, b) {
        if (action.sortName === "fileName") {
          var nameA = a[action.sortName].toLowerCase(),
            nameB = b[action.sortName].toLowerCase();
          if (nameA < nameB) return -1;
          if (nameA > nameB) return 1;
          return 0;
        } else {
          return a[action.sortName] - b[action.sortName];
        }
      });

      if (action.sortType === "desc") {
        fileArr.reverse();
      }

      return {
        ...state,
        files: fileArr,
      };
    default:
      return state;
  }
};

const initialState = {
  fileId: 0,
  files: [],
};

const FileContextProvider = (props) => {
  const [file, dispatch] = useReducer(fileReducer, initialState);

  return (
    <FileContext.Provider value={{ file, dispatch }}>
      {props.children}
    </FileContext.Provider>
  );
};

export default FileContextProvider;
