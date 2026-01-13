import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvAgeTiposTarefasesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvAgeTiposTarefases(input: GetAdvAgeTiposTarefasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advAgeTiposTarefases", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-age-tipos-tarefas", {
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

export function useAllAdvAgeTiposTarefases() {
  return useQuery({
    queryKey: ["advAgeTiposTarefases", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-age-tipos-tarefas", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvAgeTiposTarefas(id: string) {
  return useQuery({
    queryKey: ["advAgeTiposTarefas", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-age-tipos-tarefas/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvAgeTiposTarefas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-age-tipos-tarefas", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advAgeTiposTarefases"] });
    },
  });
}

export function useUpdateAdvAgeTiposTarefas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-age-tipos-tarefas/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advAgeTiposTarefases"] });
      queryClient.invalidateQueries({ queryKey: ["advAgeTiposTarefas", data.id] });
    },
  });
}

export function useDeleteAdvAgeTiposTarefas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-age-tipos-tarefas/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advAgeTiposTarefases"] });
    },
  });
}


