import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import { DataGrid, type GridColDef } from '@mui/x-data-grid';
import { EMPLOYEES_DATA_LABELS, sxStyles } from '../constants';
import { useAboveAveragePerformersQuery } from '../lib/queries';
import type { AboveAveragePerformerResponse } from '../lib/types';
import { Loader } from './Loader';
import { ErrorOccured } from './Error';
import { DataCard } from './DataCard';

const columns: GridColDef<AboveAveragePerformerResponse>[] = [
  {
    field: 'firstName',
    headerName: EMPLOYEES_DATA_LABELS.ABOVE_AVERAGE_PERFORMERS_FIRST_NAME,
    flex: 1,
  },
  {
    field: 'lastName',
    headerName: EMPLOYEES_DATA_LABELS.ABOVE_AVERAGE_PERFORMERS_LAST_NAME,
    flex: 1,
  },
  {
    field: 'jobCount',
    headerName: EMPLOYEES_DATA_LABELS.ABOVE_AVERAGE_PERFORMERS_JOB_COUNT,
    flex: 1,
    type: 'number',
  },
];

export function AboveAveragePerformersCard() {
  const { isPending, error, data } = useAboveAveragePerformersQuery();

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
    <DataCard title={EMPLOYEES_DATA_LABELS.ABOVE_AVERAGE_PERFORMERS_TITLE}>
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
