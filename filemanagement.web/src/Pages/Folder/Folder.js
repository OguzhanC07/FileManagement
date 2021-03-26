import React from "react";
import AddFolderModal from "../../components/AddFolderModal";
import FetchingData from "../../components/FetchingData";

const Folder = (props) => {
  return (
    <div>
      <AddFolderModal />
      <FetchingData />
    </div>
  );
};

export default Folder;
