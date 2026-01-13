import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFabMotivosPerdasInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFabMotivosPerdas(input: GetFabMotivosPerdasInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["fabMotivosPerdas", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-motivos-perda", {
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

export function useAllFabMotivosPerdas() {
  return useQuery({
    queryKey: ["fabMotivosPerdas", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-motivos-perda", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFabMotivosPerda(id: string) {
  return useQuery({
    queryKey: ["fabMotivosPerda", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fab-motivos-perda/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFabMotivosPerda() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fab-motivos-perda", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabMotivosPerdas"] });
    },
  });
}

export function useUpdateFabMotivosPerda() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fab-motivos-perda/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["fabMotivosPerdas"] });
      queryClient.invalidateQueries({ queryKey: ["fabMotivosPerda", data.id] });
    },
  });
}

export function useDeleteFabMotivosPerda() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fab-motivos-perda/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabMotivosPerdas"] });
    },
  });
}


