import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvCliTiposArquivosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  advClientesArquivosId?: string;
  advClientesChecklistId?: string;
  }

export function useAdvCliTiposArquivoses(input: GetAdvCliTiposArquivosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advCliTiposArquivoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-tipos-arquivos", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllAdvCliTiposArquivoses() {
  return useQuery({
    queryKey: ["advCliTiposArquivoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-tipos-arquivos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvCliTiposArquivos(id: string) {
  return useQuery({
    queryKey: ["advCliTiposArquivos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-cli-tipos-arquivos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvCliTiposArquivos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-cli-tipos-arquivos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliTiposArquivoses"] });
    },
  });
}

export function useUpdateAdvCliTiposArquivos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-cli-tipos-arquivos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advCliTiposArquivoses"] });
      queryClient.invalidateQueries({ queryKey: ["advCliTiposArquivos", data.id] });
    },
  });
}

export function useDeleteAdvCliTiposArquivos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-cli-tipos-arquivos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliTiposArquivoses"] });
    },
  });
}





/**
 * Toggle hook for many-to-many relationship: advClientesArquivos <-> advClientesChecklist
 * Given a advClientesArquivos, toggle a advClientesChecklist
 */
export function useToggleAdvClientesChecklist(advClientesArquivosId: string) {
  const queryClient = useQueryClient();
  const createMutation = useCreateAdvCliTiposArquivos();
  const deleteMutation = useDeleteAdvCliTiposArquivos();
  const { data: existing } = useAdvCliTiposArquivoses({ advClientesArquivosId: advClientesArquivosId, maxResultCount: 1000 });

  return useMutation({
    mutationFn: async ({ advClientesChecklistId, isChecked }: { advClientesChecklistId: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove relationship
        const record = existing?.items?.find((i: any) => i.advClientesChecklistId === advClientesChecklistId);
        if (record) {
          await deleteMutation.mutateAsync(record.id);
        }
      } else {
        // Add relationship
        await createMutation.mutateAsync({
          advClientesArquivosId: advClientesArquivosId,
          advClientesChecklistId: advClientesChecklistId,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliTiposArquivoses"] });
    },
  });
}

/**
 * Toggle hook for many-to-many relationship: advClientesArquivos <-> advClientesChecklist
 * Given a advClientesChecklist, toggle a advClientesArquivos
 */
export function useToggleAdvClientesArquivos(advClientesChecklistId: string) {
  const queryClient = useQueryClient();
  const createMutation = useCreateAdvCliTiposArquivos();
  const deleteMutation = useDeleteAdvCliTiposArquivos();
  const { data: existing } = useAdvCliTiposArquivoses({ advClientesChecklistId: advClientesChecklistId, maxResultCount: 1000 });

  return useMutation({
    mutationFn: async ({ advClientesArquivosId, isChecked }: { advClientesArquivosId: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove relationship
        const record = existing?.items?.find((i: any) => i.advClientesArquivosId === advClientesArquivosId);
        if (record) {
          await deleteMutation.mutateAsync(record.id);
        }
      } else {
        // Add relationship
        await createMutation.mutateAsync({
          advClientesArquivosId: advClientesArquivosId,
          advClientesChecklistId: advClientesChecklistId,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliTiposArquivoses"] });
    },
  });
}

