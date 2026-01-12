/**
 * React (Next.js) Template Generators
 * Based on abp-react-main project structure
 */

import type { EntityField } from '../../../types';

// ============ UTILITY FUNCTIONS ============

export const toCamelCase = (str: string): string =>
    str.charAt(0).toLowerCase() + str.slice(1);

export const toKebabCase = (str: string): string =>
    str.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase();

export const toPascalCase = (str: string): string =>
    str.charAt(0).toUpperCase() + str.slice(1);

// Map C# types to TypeScript types
export const toTsType = (type: string, nullable?: boolean): string => {
    const typeMap: Record<string, string> = {
        'string': 'string',
        'int': 'number',
        'long': 'number',
        'double': 'number',
        'decimal': 'number',
        'float': 'number',
        'bool': 'boolean',
        'guid': 'string',
        'datetime': 'string',
        'byte': 'number',
        'short': 'number',
        'char': 'string',
        'enum': 'number',
    };
    const tsType = typeMap[type.toLowerCase()] || 'unknown';
    return nullable ? `${tsType} | null` : tsType;
};

// Get cell renderer for TanStack Table
export const getCellRenderer = (field: EntityField): string => {
    switch (field.type) {
        case 'bool':
            return `info.getValue() ? 'Yes' : 'No'`;
        case 'datetime':
            return `info.getValue() ? new Date(info.getValue() as string).toLocaleDateString() : '-'`;
        case 'enum':
            return `info.getValue()`;
        default:
            return `info.getValue()`;
    }
};

// ============ PAGE TEMPLATE ============

export function getReactPageTemplate(): string {
    return `import { Add{{ entity.name }} } from '@/components/{{ entity.name | kebabCase }}/Add{{ entity.name }}'
import { {{ entity.name }}List } from '@/components/{{ entity.name | kebabCase }}/{{ entity.name }}List'

export default function Admin{{ entity.pluralName }}Page() {
  return (
    <div className="w-full">
      <div className="mb-4">
        <h1 className="text-2xl font-bold">{{ entity.pluralName }}</h1>
      </div>
      <Add{{ entity.name }} />
      <{{ entity.name }}List />
    </div>
  )
}
`;
}

// ============ LIST COMPONENT TEMPLATE ============

export function getReactListComponentTemplate(): string {
    return `'use client'
import { {{ entity.name }}Dto } from '@/client'
import { CustomTable } from '@/components/ui/CustomTable'
import Error from '@/components/ui/Error'
import Loader from '@/components/ui/Loader'
import { Search } from '@/components/ui/Search'
import { useToast } from '@/components/ui/use-toast'
import { QueryNames } from '@/lib/hooks/QueryConstants'
import { use{{ entity.pluralName }} } from '@/lib/hooks/use{{ entity.pluralName }}'
import { useQueryClient } from '@tanstack/react-query'
import { ColumnDef, PaginationState, getCoreRowModel, useReactTable } from '@tanstack/react-table'
import { useMemo, useState } from 'react'
import { PermissionActions } from '../permission/PermissionActions'
import { Delete{{ entity.name }} } from './Delete{{ entity.name }}'
import { {{ entity.name }}Edit } from './{{ entity.name }}Edit'

type {{ entity.name }}ActionDialogState = {
  itemId: string
  dialogType: 'edit' | 'delete'
} | null

export const {{ entity.name }}List = () => {
  const { toast } = useToast()
  const queryClient = useQueryClient()

  const [searchStr, setSearchStr] = useState<string>('')
  const [actionDialog, setActionDialog] = useState<{{ entity.name }}ActionDialogState>(null)
  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  })

  const { isLoading, data, isError, error } = use{{ entity.pluralName }}(
    pagination.pageIndex,
    pagination.pageSize,
    searchStr || undefined
  )

  const handleActionComplete = () => {
    queryClient.invalidateQueries({ queryKey: [QueryNames.Get{{ entity.pluralName }}] })
    setActionDialog(null)
  }

  const columns = useMemo(
    () =>
      get{{ entity.name }}Columns({
        onEdit: (item) =>
          setActionDialog({
            itemId: item.id!,
            dialogType: 'edit',
          }),
        onDelete: (item) =>
          setActionDialog({
            itemId: item.id!,
            dialogType: 'delete',
          }),
      }),
    []
  )

  const table = useReactTable({
    data: data?.items ?? [],
    pageCount: Math.ceil((data?.totalCount ?? 0) / pagination.pageSize),
    state: { pagination },
    columns,
    getCoreRowModel: getCoreRowModel(),
    onPaginationChange: setPagination,
    manualPagination: true,
  })

  if (isLoading) return <Loader />
  if (isError) return <Error />

  return (
    <>
      {actionDialog && (
        <>
          {actionDialog.dialogType === 'edit' && (
            <{{ entity.name }}Edit
              itemId={actionDialog.itemId}
              onDismiss={handleActionComplete}
            />
          )}
          {actionDialog.dialogType === 'delete' && (
            <Delete{{ entity.name }}
              itemId={actionDialog.itemId}
              onDismiss={handleActionComplete}
            />
          )}
        </>
      )}
      <Search onUpdate={setSearchStr} value={searchStr} />
      <CustomTable<{{ entity.name }}Dto>
        table={table}
        totalCount={data?.totalCount ?? 0}
        pageSize={pagination.pageSize}
      />
    </>
  )
}

const get{{ entity.name }}Columns = (actions: {
  onEdit: (item: {{ entity.name }}Dto) => void
  onDelete: (item: {{ entity.name }}Dto) => void
}): ColumnDef<{{ entity.name }}Dto>[] => [
  {
    header: '{{ entity.pluralName }}',
    columns: [
      {
        accessorKey: 'actions',
        header: 'Actions',
        cell: (info) => (
          <PermissionActions
            actions={[
              {
                icon: 'pencil',
                policy: '{{ project.name }}.{{ entity.name }}.Update',
                callback: () => actions.onEdit(info.row.original),
              },
              {
                icon: 'trash',
                policy: '{{ project.name }}.{{ entity.name }}.Delete',
                callback: () => actions.onDelete(info.row.original),
              },
            ]}
          />
        ),
      },
      {% for field in entity.fields %}{% if field.showInGrid != false %}
      {
        accessorKey: '{{ field.name | camelCase }}',
        header: '{{ field.label | default: field.name }}',
        cell: (info) => {% if field.type == 'bool' %}info.getValue() ? 'Yes' : 'No'{% elsif field.type == 'datetime' %}info.getValue() ? new Date(info.getValue() as string).toLocaleDateString() : '-'{% else %}info.getValue(){% endif %},
      },
      {% endif %}{% endfor %}
    ],
  },
]
`;
}

// ============ ADD COMPONENT TEMPLATE ============

export function getReactAddComponentTemplate(): string {
    return `'use client'
import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog'
import { Plus } from 'lucide-react'
import { useState } from 'react'
import { {{ entity.name }}Form } from './{{ entity.name }}Form'
import { useQueryClient } from '@tanstack/react-query'
import { QueryNames } from '@/lib/hooks/QueryConstants'

export const Add{{ entity.name }} = () => {
  const [open, setOpen] = useState(false)
  const queryClient = useQueryClient()

  const handleSuccess = () => {
    setOpen(false)
    queryClient.invalidateQueries({ queryKey: [QueryNames.Get{{ entity.pluralName }}] })
  }

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button className="mb-4">
          <Plus className="mr-2 h-4 w-4" />
          Add {{ entity.name }}
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[600px]">
        <DialogHeader>
          <DialogTitle>Create {{ entity.name }}</DialogTitle>
          <DialogDescription>
            Fill in the details below to create a new {{ entity.name | downcase }}.
          </DialogDescription>
        </DialogHeader>
        <{{ entity.name }}Form onSuccess={handleSuccess} onCancel={() => setOpen(false)} />
      </DialogContent>
    </Dialog>
  )
}
`;
}

// ============ EDIT COMPONENT TEMPLATE ============

export function getReactEditComponentTemplate(): string {
    return `'use client'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog'
import { use{{ entity.name }} } from '@/lib/hooks/use{{ entity.pluralName }}'
import { {{ entity.name }}Form } from './{{ entity.name }}Form'
import Loader from '@/components/ui/Loader'

interface {{ entity.name }}EditProps {
  itemId: string
  onDismiss: () => void
}

export const {{ entity.name }}Edit = ({ itemId, onDismiss }: {{ entity.name }}EditProps) => {
  const { data, isLoading } = use{{ entity.name }}(itemId)

  return (
    <Dialog open onOpenChange={() => onDismiss()}>
      <DialogContent className="sm:max-w-[600px]">
        <DialogHeader>
          <DialogTitle>Edit {{ entity.name }}</DialogTitle>
          <DialogDescription>
            Update the details below.
          </DialogDescription>
        </DialogHeader>
        {isLoading ? (
          <Loader />
        ) : (
          <{{ entity.name }}Form 
            initialData={data} 
            onSuccess={onDismiss} 
            onCancel={onDismiss} 
          />
        )}
      </DialogContent>
    </Dialog>
  )
}
`;
}

// ============ DELETE COMPONENT TEMPLATE ============

export function getReactDeleteComponentTemplate(): string {
    return `'use client'
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from '@/components/ui/alert-dialog'
import { useToast } from '@/components/ui/use-toast'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { {{ entity.name | camelCase }}Service } from '@/client'
import { QueryNames } from '@/lib/hooks/QueryConstants'

interface Delete{{ entity.name }}Props {
  itemId: string
  onDismiss: () => void
}

export const Delete{{ entity.name }} = ({ itemId, onDismiss }: Delete{{ entity.name }}Props) => {
  const { toast } = useToast()
  const queryClient = useQueryClient()

  const mutation = useMutation({
    mutationFn: () => {{ entity.name | camelCase }}Service.delete({ id: itemId }),
    onSuccess: () => {
      toast({ title: 'Success', description: '{{ entity.name }} deleted successfully' })
      queryClient.invalidateQueries({ queryKey: [QueryNames.Get{{ entity.pluralName }}] })
      onDismiss()
    },
    onError: (error: Error) => {
      toast({ title: 'Error', description: error.message, variant: 'destructive' })
    },
  })

  return (
    <AlertDialog open onOpenChange={() => onDismiss()}>
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>Delete {{ entity.name }}</AlertDialogTitle>
          <AlertDialogDescription>
            Are you sure you want to delete this {{ entity.name | downcase }}? This action cannot be undone.
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogCancel onClick={onDismiss}>Cancel</AlertDialogCancel>
          <AlertDialogAction
            onClick={() => mutation.mutate()}
            className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
          >
            {mutation.isPending ? 'Deleting...' : 'Delete'}
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  )
}
`;
}

// ============ FORM COMPONENT TEMPLATE ============

export function getReactFormComponentTemplate(): string {
    return `'use client'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Textarea } from '@/components/ui/textarea'
import { Checkbox } from '@/components/ui/checkbox'
import { useToast } from '@/components/ui/use-toast'
import { useMutation } from '@tanstack/react-query'
import { {{ entity.name | camelCase }}Service, {{ entity.name }}Dto, CreateUpdate{{ entity.name }}Dto } from '@/client'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'

const formSchema = z.object({
  {% for field in entity.fields %}{% if field.showInForm != false %}
  {{ field.name | camelCase }}: {% if field.type == 'string' %}z.string(){% if field.isRequired %}.min(1, '{{ field.label | default: field.name }} is required'){% else %}.optional(){% endif %}{% if field.maxLength %}.max({{ field.maxLength }}){% endif %}{% elsif field.type == 'int' or field.type == 'long' or field.type == 'double' or field.type == 'decimal' %}z.number(){% if field.isRequired %}{% else %}.optional(){% endif %}{% elsif field.type == 'bool' %}z.boolean(){% elsif field.type == 'datetime' %}z.string(){% if field.isRequired %}.min(1){% else %}.optional(){% endif %}{% elsif field.type == 'guid' %}z.string(){% if field.isRequired %}.uuid(){% else %}.optional(){% endif %}{% else %}z.any(){% endif %},
  {% endif %}{% endfor %}
})

type FormData = z.infer<typeof formSchema>

interface {{ entity.name }}FormProps {
  initialData?: {{ entity.name }}Dto
  onSuccess: () => void
  onCancel: () => void
}

export const {{ entity.name }}Form = ({ initialData, onSuccess, onCancel }: {{ entity.name }}FormProps) => {
  const { toast } = useToast()
  const isEdit = !!initialData?.id

  const {
    register,
    handleSubmit,
    formState: { errors },
    setValue,
    watch,
  } = useForm<FormData>({
    resolver: zodResolver(formSchema),
    defaultValues: initialData ? {
      {% for field in entity.fields %}{% if field.showInForm != false %}
      {{ field.name | camelCase }}: initialData.{{ field.name | camelCase }}{% if field.type == 'bool' %} ?? false{% endif %},
      {% endif %}{% endfor %}
    } : {
      {% for field in entity.fields %}{% if field.showInForm != false %}
      {{ field.name | camelCase }}: {% if field.type == 'bool' %}false{% elsif field.type == 'string' %}''{% elsif field.type == 'int' or field.type == 'long' or field.type == 'double' or field.type == 'decimal' %}0{% else %}undefined{% endif %},
      {% endif %}{% endfor %}
    },
  })

  const mutation = useMutation({
    mutationFn: (data: CreateUpdate{{ entity.name }}Dto) =>
      isEdit
        ? {{ entity.name | camelCase }}Service.update({ id: initialData!.id!, requestBody: data })
        : {{ entity.name | camelCase }}Service.create({ requestBody: data }),
    onSuccess: () => {
      toast({ title: 'Success', description: \`{{ entity.name }} \${isEdit ? 'updated' : 'created'} successfully\` })
      onSuccess()
    },
    onError: (error: Error) => {
      toast({ title: 'Error', description: error.message, variant: 'destructive' })
    },
  })

  const onSubmit = (data: FormData) => {
    mutation.mutate(data as CreateUpdate{{ entity.name }}Dto)
  }

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      {% for field in entity.fields %}{% if field.showInForm != false %}
      <div className="space-y-2">
        <Label htmlFor="{{ field.name | camelCase }}">{{ field.label | default: field.name }}{% if field.isRequired %} *{% endif %}</Label>
        {% if field.type == 'bool' %}
        <div className="flex items-center space-x-2">
          <Checkbox
            id="{{ field.name | camelCase }}"
            checked={watch('{{ field.name | camelCase }}')}
            onCheckedChange={(checked) => setValue('{{ field.name | camelCase }}', checked as boolean)}
          />
          <label htmlFor="{{ field.name | camelCase }}" className="text-sm">
            {{ field.placeholder | default: field.label | default: field.name }}
          </label>
        </div>
        {% elsif field.isTextArea %}
        <Textarea
          id="{{ field.name | camelCase }}"
          placeholder="{{ field.placeholder | default: '' }}"
          {...register('{{ field.name | camelCase }}')}
        />
        {% elsif field.type == 'datetime' %}
        <Input
          id="{{ field.name | camelCase }}"
          type="datetime-local"
          {...register('{{ field.name | camelCase }}')}
        />
        {% elsif field.type == 'int' or field.type == 'long' or field.type == 'double' or field.type == 'decimal' %}
        <Input
          id="{{ field.name | camelCase }}"
          type="number"
          step="{{ field.type == 'double' or field.type == 'decimal' ? '0.01' : '1' }}"
          placeholder="{{ field.placeholder | default: '' }}"
          {...register('{{ field.name | camelCase }}', { valueAsNumber: true })}
        />
        {% else %}
        <Input
          id="{{ field.name | camelCase }}"
          type="text"
          placeholder="{{ field.placeholder | default: '' }}"
          {...register('{{ field.name | camelCase }}')}
        />
        {% endif %}
        {errors.{{ field.name | camelCase }} && (
          <p className="text-sm text-destructive">{errors.{{ field.name | camelCase }}.message}</p>
        )}
      </div>
      {% endif %}{% endfor %}

      <div className="flex justify-end space-x-2 pt-4">
        <Button type="button" variant="outline" onClick={onCancel}>
          Cancel
        </Button>
        <Button type="submit" disabled={mutation.isPending}>
          {mutation.isPending ? 'Saving...' : isEdit ? 'Update' : 'Create'}
        </Button>
      </div>
    </form>
  )
}
`;
}

// ============ HOOK TEMPLATE ============

export function getReactHookTemplate(): string {
    return `import { useQuery } from '@tanstack/react-query'
import { {{ entity.name | camelCase }}Service } from '@/client'
import { QueryNames } from './QueryConstants'

export const use{{ entity.pluralName }} = (
  pageIndex: number,
  pageSize: number,
  filter?: string
) => {
  return useQuery({
    queryKey: [QueryNames.Get{{ entity.pluralName }}, pageIndex, pageSize, filter],
    queryFn: () =>
      {{ entity.name | camelCase }}Service.getList({
        skipCount: pageIndex * pageSize,
        maxResultCount: pageSize,
        filter,
      }),
  })
}

export const use{{ entity.name }} = (id: string) => {
  return useQuery({
    queryKey: [QueryNames.Get{{ entity.name }}, id],
    queryFn: () => {{ entity.name | camelCase }}Service.get({ id }),
    enabled: !!id,
  })
}
`;
}

// ============ QUERY CONSTANTS TEMPLATE ============

export function getReactQueryConstantsTemplate(): string {
    return `// Query names for TanStack Query cache invalidation
// Add new query names here as you add entities

export const QueryNames = {
  // System
  GetUsers: 'GetUsers',
  GetRoles: 'GetRoles',
  GetTenants: 'GetTenants',
  
  // Generated Entities
  {% for entityItem in entities %}
  Get{{ entityItem.data.pluralName }}: 'Get{{ entityItem.data.pluralName }}',
  Get{{ entityItem.data.name }}: 'Get{{ entityItem.data.name }}',
  {% endfor %}
} as const
`;
}
