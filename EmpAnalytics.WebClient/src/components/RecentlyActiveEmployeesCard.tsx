import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import { DataGrid, type GridColDef } from '@mui/x-data-grid';
import { EMPLOYEES_DATA_LABELS, sxStyles } from '../constants';
import { useRecentlyActiveEmployeesQuery } from '../lib/queries';
import type { RecentlyActiveEmployeeResponse } from '../lib/types';
import { formatDate } from '../utils/date';
import { Loader } from './Loader';
import { ErrorOccured } from './Error';
import { DataCard } from './DataCard';

const columns: GridColDef<RecentlyActiveEmployeeResponse>[] = [
  {
    field: 'firstName',
    headerName: EMPLOYEES_DATA_LABELS.RECENTLY_ACTIVE_EMPLOYEES_FIRST_NAME,
    flex: 1,
  },
  {
    field: 'lastName',
    headerName: EMPLOYEES_DATA_LABELS.RECENTLY_ACTIVE_EMPLOYEES_LAST_NAME,
    flex: 1,
  },
  {
    field: 'jobName',
    headerName: EMPLOYEES_DATA_LABELS.RECENTLY_ACTIVE_EMPLOYEES_JOB_NAME,
    flex: 1.5,
  },
  {
    field: 'lastJobDate',
    headerName: EMPLOYEES_DATA_LABELS.RECENTLY_ACTIVE_EMPLOYEES_LAST_JOB,
    flex: 1,
    valueFormatter: (value: string) => {
      return formatDate(value);
    },
  },
];

export function RecentlyActiveEmployeesCard() {
  const { isPending, error, data } = useRecentlyActiveEmployeesQuery();

  if (isPending) {
    return (
      <Loader />
    );
  }

  if (error) {
    return (
      <ErrorOccured error={error} />
    );
  }

  const rowsWithId = data.map((row, index) => ({
    ...row,
    id: `${row.firstName}-${row.lastName}-${index}`,
  }));

  return (
    <DataCard title={EMPLOYEES_DATA_LABELS.RECENTLY_ACTIVE_EMPLOYEES_TITLE}>
      <Paper elevation={0} sx={sxStyles.cardPaper}>
        <Box sx={sxStyles.tableContainer}>
          <DataGrid
            rows={rowsWithId}
            columns={columns}
            initialState={{
              pagination: {
                paginationModel: {
                  pageSize: 5,
                },
              },
            }}
            pageSizeOptions={[5, 10]}
            disableRowSelectionOnClick
            sx={sxStyles.dataGrid}
          />
        </Box>
      </Paper>
    </DataCard>
  );
}
