import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvTarefasesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvTarefases(input: GetAdvTarefasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advTarefases", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-tarefas", {
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

export function useAllAdvTarefases() {
  return useQuery({
    queryKey: ["advTarefases", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-tarefas", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvTarefas(id: string) {
  return useQuery({
    queryKey: ["advTarefas", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-tarefas/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvTarefas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-tarefas", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advTarefases"] });
    },
  });
}

export function useUpdateAdvTarefas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-tarefas/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advTarefases"] });
      queryClient.invalidateQueries({ queryKey: ["advTarefas", data.id] });
    },
  });
}

export function useDeleteAdvTarefas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-tarefas/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advTarefases"] });
    },
  });
}


