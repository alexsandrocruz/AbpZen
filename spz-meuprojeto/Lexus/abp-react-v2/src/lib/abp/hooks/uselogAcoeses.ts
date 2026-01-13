import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLogAcoesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useLogAcoeses(input: GetLogAcoesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["logAcoeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/log-acoes", {
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

export function useAllLogAcoeses() {
  return useQuery({
    queryKey: ["logAcoeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/log-acoes", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLogAcoes(id: string) {
  return useQuery({
    queryKey: ["logAcoes", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/log-acoes/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLogAcoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/log-acoes", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["logAcoeses"] });
    },
  });
}

export function useUpdateLogAcoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/log-acoes/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["logAcoeses"] });
      queryClient.invalidateQueries({ queryKey: ["logAcoes", data.id] });
    },
  });
}

export function useDeleteLogAcoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/log-acoes/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["logAcoeses"] });
    },
  });
}


