import { BrowserRouter as Switch, Route } from "react-router-dom";
import Folder from "../Pages/Folder/Folder";

const FolderRoute = () => {
  return (
    <Switch>
      <Route path="/" component={Folder} />
    </Switch>
  );
};

export default FolderRoute;
