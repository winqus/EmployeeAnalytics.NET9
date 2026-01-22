import {
  AppBar,
  Toolbar,
  Typography,
  Container,
  Box,
} from '@mui/material';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import { TYPOGRAPHY, THEME_PALETTE, sxStyles, APPBAR_TITLE } from './constants';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { RecentlyActiveEmployeesCard } from './components/RecentlyActiveEmployeesCard';
import { AboveAveragePerformersCard } from './components/AboveAveragePerformersCard';
import { EmployeesExceedingOwnAverageCard } from './components/EmployeesExceedingOwnAverageCard';

const queryClient = new QueryClient()

const theme = createTheme({
  palette: THEME_PALETTE,
  typography: {
    fontFamily: TYPOGRAPHY.fontFamily,
    h5: {
      fontWeight: TYPOGRAPHY.fontWeight.heavy,
      textTransform: 'uppercase',
      letterSpacing: TYPOGRAPHY.letterSpacing.normal,
    },
    h6: {
      fontWeight: TYPOGRAPHY.fontWeight.bold,
      letterSpacing: TYPOGRAPHY.letterSpacing.tight,
    },
  },
  components: {
    MuiAppBar: {
      styleOverrides: {
        root: {
          boxShadow: 'none',
        },
      },
    },
    MuiPaper: {
      styleOverrides: {
        root: {
          borderRadius: 4,
        },
      },
    },
  },
});

function App() {
  return (
    <ThemeProvider theme={theme}>
      <QueryClientProvider client={queryClient}>
        <Content />
      </QueryClientProvider>
    </ThemeProvider>
  )
}


function Content() {
  return (
    <Box sx={sxStyles.pageRoot}>
      <AppBar position="static" sx={sxStyles.appBar}>
        <Toolbar sx={sxStyles.toolbar}>
          <Typography
            variant="h6"
            component="div"
            sx={sxStyles.appBarTitle}
          >
            {APPBAR_TITLE}
          </Typography>
        </Toolbar>
      </AppBar>

      <Container maxWidth={false} sx={sxStyles.mainContainer}>
        <Box sx={sxStyles.contentStack}>
          <RecentlyActiveEmployeesCard />
          <AboveAveragePerformersCard />
          <EmployeesExceedingOwnAverageCard />
        </Box>
      </Container>
    </Box>
  )
}

export default App
