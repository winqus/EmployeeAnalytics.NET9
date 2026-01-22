import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import { BarChart } from '@mui/x-charts/BarChart';
import { COLORS, EMPLOYEES_DATA_LABELS, sxStyles } from '../constants';
import { useExceedingOwnAverageQuery } from '../lib/queries';
import { Loader } from './Loader';
import { ErrorOccured } from './Error';
import { DataCard } from './DataCard';

export function EmployeesExceedingOwnAverageCard() {
  const { isPending, error, data } = useExceedingOwnAverageQuery();

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

  const employeeNames = data.map(
    (employee) => `${employee.firstName} ${employee.lastName}`
  );
  const jobCounts = data.map((employee) => employee.jobCountLast30Days);

  return (
    <DataCard title={EMPLOYEES_DATA_LABELS.EMPLOYEES_EXCEEDING_OWN_AVERAGE_TITLE}>
      <Paper elevation={0} sx={sxStyles.chartPaper}>
        <Box sx={sxStyles.chartContainer}>
          <BarChart
            xAxis={[
              {
                id: 'employees',
                data: employeeNames,
                scaleType: 'band',
              },
            ]}
            series={[
              {
                data: jobCounts,
                label: EMPLOYEES_DATA_LABELS.EMPLOYEES_EXCEEDING_OWN_AVERAGE_SERIES,
                color: COLORS.primary,
              },
            ]}
            height={280}
          />
        </Box>
      </Paper>
    </DataCard>
  );
}
