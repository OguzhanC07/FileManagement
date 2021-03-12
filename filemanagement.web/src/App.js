import AuthContextProvider from "./context/AuthContext";
import FileContextProvider from "./context/FileContext";
import FolderContextProvider from "./context/FolderContext";
import CollectionContainer from "./routes/CollectionContainer";

function App() {
  return (
    <AuthContextProvider>
      <FolderContextProvider>
        <FileContextProvider>
          <CollectionContainer />
        </FileContextProvider>
      </FolderContextProvider>
    </AuthContextProvider>
  );
}
export default App;
