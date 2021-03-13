import React from "react";
import AddFolderModal from "../../components/AddFolderModal";
import FetchingData from "../../components/FetchingData";
import Layout from "../../components/Layout";

const Folder = (props) => {
  return (
    <Layout>
      <AddFolderModal />
      <FetchingData />
    </Layout>
  );
};

export default Folder;
