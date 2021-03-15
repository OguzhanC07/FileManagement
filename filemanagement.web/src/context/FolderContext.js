import React, { createContext, useReducer } from "react";

export const SETFOLDERS = "SETFOLDERS";
export const ADDFOLDER = "ADDFOLDER";
export const EDITFOLDER = "EDITFOLDER";
export const DELETEFOLDER = "DELETEFOLDER";
export const REMOVEFROMFOLDERSTACK = "REMOVEFROMFOLDERSTACK";
export const ADDTOFOLDERSTACK = "ADDTOFOLDERSTACK";

export const FolderContext = createContext();

const folderReducer = (state, action) => {
  switch (action.type) {
    case SETFOLDERS:
      return {
        ...state,
        folders: action.folders,
      };
    case ADDFOLDER:
      return {
        ...state,
        folders: state.folders.concat(action.addedFolder),
      };
    case EDITFOLDER:
      const index = state.folders.findIndex((fold) => fold.id === action.fid);
      const newArr = [...state.folders];
      newArr[index].folderName = action.name;
      return {
        ...state,
        folders: newArr,
      };
    case DELETEFOLDER:
      const filteredFolders = state.folders.filter(
        (fold) => fold.id !== action.fid
      );
      return {
        ...state,
        folders: filteredFolders,
      };
    case REMOVEFROMFOLDERSTACK:
      const newStackArr = [...state.folderStack];
      const stackIndex = newStackArr.findIndex(
        (stck) => stck.id === action.fid
      );
      newStackArr.splice(stackIndex + 1);
      return {
        ...state,
        folderId: action.fid,
        folderStack: newStackArr,
      };
    case ADDTOFOLDERSTACK:
      return {
        ...state,
        folderId: action.folderInfo.id,
        folderStack: [...state.folderStack, action.folderInfo],
      };
    default:
      return state;
  }
};

const initialState = {
  folderId: 0,
  folders: [],
  folderStack: [],
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
