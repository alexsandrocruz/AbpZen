import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetOpoTiposesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useOpoTiposes(input: GetOpoTiposesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["opoTiposes", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/opo-tipos", {
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

export function useAllOpoTiposes() {
  return useQuery({
    queryKey: ["opoTiposes", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/opo-tipos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useOpoTipos(id: string) {
  return useQuery({
    queryKey: ["opoTipos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/opo-tipos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateOpoTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/opo-tipos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["opoTiposes"] });
    },
  });
}

export function useUpdateOpoTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/opo-tipos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["opoTiposes"] });
      queryClient.invalidateQueries({ queryKey: ["opoTipos", data.id] });
    },
  });
}

export function useDeleteOpoTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/opo-tipos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["opoTiposes"] });
    },
  });
}


