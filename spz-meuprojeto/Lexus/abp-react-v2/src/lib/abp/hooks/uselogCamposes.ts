import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLogCamposesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useLogCamposes(input: GetLogCamposesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["logCamposes", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/log-campos", {
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

export function useAllLogCamposes() {
  return useQuery({
    queryKey: ["logCamposes", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/log-campos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLogCampos(id: string) {
  return useQuery({
    queryKey: ["logCampos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/log-campos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLogCampos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/log-campos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["logCamposes"] });
    },
  });
}

export function useUpdateLogCampos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/log-campos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["logCamposes"] });
      queryClient.invalidateQueries({ queryKey: ["logCampos", data.id] });
    },
  });
}

export function useDeleteLogCampos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/log-campos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["logCamposes"] });
    },
  });
}


