import { useState, useMemo } from 'react';
import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import Stack from '@mui/material/Stack';
import { DataGrid, type GridColDef } from '@mui/x-data-grid';
import { EMPLOYEES_DATA_LABELS, sxStyles } from '../constants';
import { useAboveAveragePerformersQuery } from '../lib/queries';
import type { AboveAverageFilters, AboveAveragePerformerResponse } from '../lib/types';
import { Loader } from './Loader';
import { ErrorOccured } from './Error';
import { DataCard } from './DataCard';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import type { Dayjs } from 'dayjs';

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
  const [startDate, setStartDate] = useState<Dayjs | null>(null);
  const [endDate, setEndDate] = useState<Dayjs | null>(null);

  const filters: AboveAverageFilters | undefined = useMemo(() => {
    if (!startDate && !endDate) return undefined;
    return {
      startDate: startDate?.format('YYYY-MM-DD'),
      endDate: endDate?.format('YYYY-MM-DD'),
    };
  }, [startDate, endDate]);

  const { isPending, error, data } = useAboveAveragePerformersQuery(filters);

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
        <Stack
          direction={{ xs: 'column', sm: 'row' }}
          spacing={2}
          sx={{ p: 2, pb: 0 }}
        >
          <DatePicker
            label={EMPLOYEES_DATA_LABELS.ABOVE_AVERAGE_PERFORMERS_START_DATE}
            value={startDate}
            onChange={(newValue) => setStartDate(newValue)}
            maxDate={endDate ?? undefined}
            slotProps={{
              textField: { size: 'small' },
              field: { clearable: true },
            }}
          />
          <DatePicker
            label={EMPLOYEES_DATA_LABELS.ABOVE_AVERAGE_PERFORMERS_END_DATE}
            value={endDate}
            onChange={(newValue) => setEndDate(newValue)}
            minDate={startDate ?? undefined}
            slotProps={{
              textField: { size: 'small' },
              field: { clearable: true },
            }}
          />
        </Stack>

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
