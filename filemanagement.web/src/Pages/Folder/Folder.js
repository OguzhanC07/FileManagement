import React from "react";
import AddFolderModal from "../../components/AddFolderModal";
import FetchingData from "../../components/FetchingData";
import Layout from "../../components/Layout";

const Folder = (props) => {
  const addFolderHandler = (name) => {
    console.log(name);
  };

  return (
    <Layout>
      <AddFolderModal addfolder={addFolderHandler} />
      <FetchingData />
    </Layout>
  );
};

export default Folder;
