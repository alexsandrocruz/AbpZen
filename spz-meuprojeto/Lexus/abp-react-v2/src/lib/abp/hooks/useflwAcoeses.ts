import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFlwAcoesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFlwAcoeses(input: GetFlwAcoesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["flwAcoeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/flw-acoes", {
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

export function useAllFlwAcoeses() {
  return useQuery({
    queryKey: ["flwAcoeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/flw-acoes", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFlwAcoes(id: string) {
  return useQuery({
    queryKey: ["flwAcoes", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/flw-acoes/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFlwAcoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/flw-acoes", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["flwAcoeses"] });
    },
  });
}

export function useUpdateFlwAcoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/flw-acoes/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["flwAcoeses"] });
      queryClient.invalidateQueries({ queryKey: ["flwAcoes", data.id] });
    },
  });
}

export function useDeleteFlwAcoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/flw-acoes/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["flwAcoeses"] });
    },
  });
}


