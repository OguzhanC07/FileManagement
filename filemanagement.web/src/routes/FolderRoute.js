import Folder from "../Pages/Folder/Folder";
import Layout from "../components/Layout";
import Folder2 from "../Pages/Folder/Folder2";
import { BrowserRouter, Route } from "react-router-dom";

const FolderRoute = () => {
  return (
    <Layout>
      <BrowserRouter>
        <Route path="/" component={Folder} />
        <Route path="/folder" component={Folder2} />
      </BrowserRouter>
    </Layout>
  );
};

export default FolderRoute;
