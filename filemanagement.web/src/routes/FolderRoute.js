import { BrowserRouter, Route } from "react-router-dom";
import MainContent from "../components/MainContent";

import NavbarAndSidebar from "../components/NavbarAndSidebar";
import About from "../Pages/Folder/About";
import Folder from "../Pages/Folder/Folder";
import "../styles/admin.css";

const FolderRoute = () => {
  return (
    <BrowserRouter>
      <NavbarAndSidebar />
      <MainContent>
        <Route exact path="/" component={Folder} />
        <Route path="/about" component={About} />
      </MainContent>
    </BrowserRouter>
  );
};

export default FolderRoute;
