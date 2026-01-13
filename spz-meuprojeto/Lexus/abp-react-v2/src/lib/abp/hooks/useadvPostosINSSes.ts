import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvPostosINSSesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvPostosINSSes(input: GetAdvPostosINSSesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advPostosINSSes", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-postos-inss", {
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

export function useAllAdvPostosINSSes() {
  return useQuery({
    queryKey: ["advPostosINSSes", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-postos-inss", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvPostosINSS(id: string) {
  return useQuery({
    queryKey: ["advPostosINSS", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-postos-inss/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvPostosINSS() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-postos-inss", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPostosINSSes"] });
    },
  });
}

export function useUpdateAdvPostosINSS() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-postos-inss/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advPostosINSSes"] });
      queryClient.invalidateQueries({ queryKey: ["advPostosINSS", data.id] });
    },
  });
}

export function useDeleteAdvPostosINSS() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-postos-inss/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPostosINSSes"] });
    },
  });
}


