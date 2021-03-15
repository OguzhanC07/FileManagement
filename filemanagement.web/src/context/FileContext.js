import React, { useReducer, createContext } from "react";

export const SETFILES = "SETFILES";
export const REMOVEFILES = "REMOVEFILES";
export const EDITFILE = "EDITFILE";
export const DELETEFILE = "DELETEFILE";

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
