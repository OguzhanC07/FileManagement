import Folder from "../Pages/Folder/Folder";
import { BrowserRouter, Route } from "react-router-dom";

const FolderRoute = () => {
  return (
    <BrowserRouter>
      <Route path="/" component={Folder} />
    </BrowserRouter>
  );
};

export default FolderRoute;
