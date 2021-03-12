import React, { useReducer, createContext } from "react";

export const FileContext = createContext();

const fileReducer = (state, action) => {
  switch (action.type) {
    default:
      return state;
  }
};

const initialState = {
  fileId: 0,
  folderId: 0,
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
