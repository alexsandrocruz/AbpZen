import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvTarefasAtualizacoesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvTarefasAtualizacoeses(input: GetAdvTarefasAtualizacoesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advTarefasAtualizacoeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-tarefas-atualizacoes", {
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

export function useAllAdvTarefasAtualizacoeses() {
  return useQuery({
    queryKey: ["advTarefasAtualizacoeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-tarefas-atualizacoes", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvTarefasAtualizacoes(id: string) {
  return useQuery({
    queryKey: ["advTarefasAtualizacoes", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-tarefas-atualizacoes/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvTarefasAtualizacoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-tarefas-atualizacoes", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advTarefasAtualizacoeses"] });
    },
  });
}

export function useUpdateAdvTarefasAtualizacoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-tarefas-atualizacoes/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advTarefasAtualizacoeses"] });
      queryClient.invalidateQueries({ queryKey: ["advTarefasAtualizacoes", data.id] });
    },
  });
}

export function useDeleteAdvTarefasAtualizacoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-tarefas-atualizacoes/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advTarefasAtualizacoeses"] });
    },
  });
}


