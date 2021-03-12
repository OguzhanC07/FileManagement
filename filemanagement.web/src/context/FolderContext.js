import React, { createContext, useReducer } from "react";

export const FolderContext = createContext();

const folderReducer = (state, action) => {
  switch (action.type) {
    default:
      return state;
  }
};

const initialState = {
  folderId: 0,
  folders: [],
  folderStack: [{}],
};

const FolderContextProvider = (props) => {
  const [folder, dispatch] = useReducer(folderReducer, initialState);

  return (
    <FolderContext.Provider value={{ folder, dispatch }}>
      {props.children}
    </FolderContext.Provider>
  );
};

export default FolderContextProvider;
