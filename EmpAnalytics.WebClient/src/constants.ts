import type { SxProps, Theme } from '@mui/material/styles';

export const COLORS = {
    primary: '#001A77',
    primaryDark: '#041D79',
    primaryMuted: '#6B7AA7',

    background: '#FFFFFF',
    panel: '#F0F1F3',
    rowHover: '#F9FAFB',

    border: '#D9DDE3',

    text: '#1F2937',
    textMuted: '#6B7280',
    textWhite: '#FFFFFF',
} as const;

export const TYPOGRAPHY = {
    fontFamily: '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Helvetica, Arial, sans-serif',
    letterSpacing: {
        tight: '0.01em',
        normal: '0.02em',
        wide: '0.03em',
    },
    fontWeight: {
        medium: 500,
        semibold: 600,
        bold: 700,
        heavy: 800,
    },
    fontSize: {
        xs: '0.75rem',
        sm: '0.875rem',
        base: '1rem',
    },
} as const;

export const SPACING = {
    containerMaxWidth: '1000px',
    borderRadius: '4px',
    toolbarMinHeight: 56,
} as const;

export const THEME_PALETTE = {
    primary: {
        main: COLORS.primary,
        dark: COLORS.primaryDark,
    },
    background: {
        default: COLORS.background,
        paper: COLORS.background,
    },
    text: {
        primary: COLORS.text,
        secondary: COLORS.textMuted,
    },
} as const;

export const sxStyles = {
    pageRoot: {
        display: 'flex',
        flexDirection: 'column',
        minHeight: '100vh',
        bgcolor: COLORS.background,
    } as SxProps<Theme>,

    centeredLoader: {
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        minHeight: '100vh',
        color: 'text.secondary',
    } as SxProps<Theme>,

    centeredError: {
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        minHeight: '100vh',
        color: 'error.main',
    } as SxProps<Theme>,

    appBar: {
        bgcolor: COLORS.primary,
        borderRadius: 0,
    } as SxProps<Theme>,

    toolbar: {
        minHeight: SPACING.toolbarMinHeight,
    } as SxProps<Theme>,

    appBarTitle: {
        flexGrow: 1,
        color: COLORS.textWhite,
        fontWeight: TYPOGRAPHY.fontWeight.bold,
        letterSpacing: TYPOGRAPHY.letterSpacing.normal,
        textTransform: 'uppercase',
    } as SxProps<Theme>,

    mainContainer: {
        maxWidth: SPACING.containerMaxWidth,
        width: '100%',
        py: 5,
        px: { xs: 2, sm: 3 },
    } as SxProps<Theme>,

    contentStack: {
        display: 'flex',
        flexDirection: 'column',
        gap: 4,
    } as SxProps<Theme>,

    panelPaper: {
        bgcolor: COLORS.panel,
        border: `1px solid ${COLORS.border}`,
        p: 2.5,
        borderRadius: SPACING.borderRadius,
    } as SxProps<Theme>,

    cardPaper: {
        border: `1px solid ${COLORS.border}`,
        borderRadius: SPACING.borderRadius,
        overflow: 'hidden',
    } as SxProps<Theme>,

    chartPaper: {
        border: `1px solid ${COLORS.border}`,
        borderRadius: SPACING.borderRadius,
        p: 2,
    } as SxProps<Theme>,

    sectionHeading: {
        color: COLORS.primary,
        fontSize: TYPOGRAPHY.fontSize.base,
        fontWeight: TYPOGRAPHY.fontWeight.heavy,
        textTransform: 'uppercase',
        letterSpacing: TYPOGRAPHY.letterSpacing.normal,
        mb: 2,
    } as SxProps<Theme>,

    panelHeading: {
        color: COLORS.primary,
        fontSize: TYPOGRAPHY.fontSize.base,
        fontWeight: TYPOGRAPHY.fontWeight.heavy,
        textTransform: 'uppercase',
        letterSpacing: TYPOGRAPHY.letterSpacing.normal,
        mb: 1,
    } as SxProps<Theme>,

    primaryText: {
        color: COLORS.text,
        fontWeight: TYPOGRAPHY.fontWeight.medium,
    } as SxProps<Theme>,

    mutedText: {
        color: COLORS.textMuted,
        fontSize: TYPOGRAPHY.fontSize.sm,
        mt: 0.5,
    } as SxProps<Theme>,

    dataGrid: {
        border: 'none',
        '& .MuiDataGrid-columnHeaders': {
            backgroundColor: COLORS.panel,
            color: COLORS.primary,
            fontWeight: TYPOGRAPHY.fontWeight.bold,
            textTransform: 'uppercase',
            fontSize: TYPOGRAPHY.fontSize.xs,
            letterSpacing: TYPOGRAPHY.letterSpacing.wide,
        },
        '& .MuiDataGrid-columnHeaderTitle': {
            fontWeight: TYPOGRAPHY.fontWeight.bold,
        },
        '& .MuiDataGrid-cell': {
            borderColor: COLORS.border,
            color: COLORS.text,
        },
        '& .MuiDataGrid-row:hover': {
            backgroundColor: COLORS.rowHover,
        },
        '& .MuiDataGrid-footerContainer': {
            borderTop: `1px solid ${COLORS.border}`,
            backgroundColor: COLORS.panel,
        },
    } as SxProps<Theme>,

    chartContainer: {
        height: 300,
        width: '100%',
    } as SxProps<Theme>,

    tableContainer: {
        height: 400,
        width: '100%',
    } as SxProps<Theme>,
} as const;

export const LOADING_MESSAGE = 'Loading...';
export const APPBAR_TITLE = 'Employee Activity AnalyticsEmployee Activity Analytics';
export const EMPLOYEES_DATA_LABELS = {
    RECENTLY_ACTIVE_EMPLOYEES_TITLE: 'Recently Active Employees',
    RECENTLY_ACTIVE_EMPLOYEES_FIRST_NAME: 'FIRST NAME',
    RECENTLY_ACTIVE_EMPLOYEES_LAST_NAME: 'LAST NAME',
    RECENTLY_ACTIVE_EMPLOYEES_JOB_NAME: 'JOB NAME',
    RECENTLY_ACTIVE_EMPLOYEES_LAST_JOB: 'LAST JOB DATE',
    ABOVE_AVERAGE_PERFORMERS_TITLE: 'Above Average Performers',
    ABOVE_AVERAGE_PERFORMERS_FIRST_NAME: 'FIRST NAME',
    ABOVE_AVERAGE_PERFORMERS_LAST_NAME: 'LAST NAME',
    ABOVE_AVERAGE_PERFORMERS_JOB_COUNT: 'JOB COUNT',
    EMPLOYEES_EXCEEDING_OWN_AVERAGE_TITLE: 'Employees Exceeding Own Average',
    EMPLOYEES_EXCEEDING_OWN_AVERAGE_SERIES: 'Jobs (Last 30 Days)',
};